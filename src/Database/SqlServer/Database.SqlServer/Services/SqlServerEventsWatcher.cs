using System.Data;
using Coravel.Queuing.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Jobs.Queuing;
using Sqliste.Core.Models.Events;
using Sqliste.Database.SqlServer.Configuration;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerEventsWatcher : IDisposable, IDatabaseEventWatcher
{
    private SqlConnection? _sqlConnection;
    private SqlDependency? _sqlDependency;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SqlServerEventsWatcher> _logger;
    private readonly IOptionsMonitor<SqlServerConfiguration> _configurationMonitor;
    private readonly List<EventModel> _events = new();
    private readonly IQueue _queue;

    private const string SelectEventsQuery 
        = "SELECT [id], [type], [name], [args], [inserted_at] FROM [sqliste].[app_events] WHERE [ID] > @Id";

    private long? _lastMaxId = null;

    public SqlServerEventsWatcher(IServiceScopeFactory scopeFactory,
        ILogger<SqlServerEventsWatcher> logger, IOptionsMonitor<SqlServerConfiguration> configurationMonitor, IQueue queue)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configurationMonitor = configurationMonitor;
        _queue = queue;
    }

    public void Init()
    {
        _configurationMonitor.OnChange(OnConfigChange);
        OnConfigChange(_configurationMonitor.CurrentValue);
    }

    private void OnConfigChange(SqlServerConfiguration configuration)
    {
        SqlDependency.Start(configuration.ConnectionString);

        _sqlConnection?.Close();
        _sqlConnection?.Dispose();

        _sqlConnection = new SqlConnection(configuration.ConnectionString);
        _sqlConnection.Open();

        SetupDependency();
    }

    private void OnDependencyChange(object sender, SqlNotificationEventArgs eventArgs)
    {
        if (_sqlDependency != null)
            _sqlDependency.OnChange -= OnDependencyChange;

        SqlNotificationInfo sqlNotificationInfo = eventArgs.Info;
        if (sqlNotificationInfo == SqlNotificationInfo.Truncate)
            _lastMaxId = -1;

        SetupDependency();
    }

    private void SetupDependency()
    {
        if (_sqlConnection == null) 
            return;

        using SqlCommand sqlCommand = new(SelectEventsQuery, _sqlConnection);
        sqlCommand.Parameters.AddWithValue("@Id", _lastMaxId ?? -1);

        _sqlDependency = new SqlDependency(sqlCommand);
        _sqlDependency.OnChange += OnDependencyChange;
        
        using SqlDataReader reader = sqlCommand.ExecuteReader();
        _events.Clear();

        if (!reader.HasRows) 
            return;

        while (reader.Read())
        {
            EventModel model = new()
            {
                Id = reader.GetInt64(0),
                Type = reader.GetString(1),
                Name = reader.GetString(2),
                Args = reader.IsDBNull(3) ? null : reader.GetString(3),
                InsertedAt = reader.GetDateTime(4),
            };

            _events.Add(model);
        }

        if (_lastMaxId.HasValue)
            NotifyEvents();

        _lastMaxId = _events.Max(e => e.Id);
    }

    private void NotifyEvents()
    {
        _queue.QueueInvocableWithPayload<DatabaseEventInvocable, DatabaseEventInvocablePayload>(
            new DatabaseEventInvocablePayload()
            {
                Events = new List<EventModel>(_events),
            });
    }

    public void Dispose()
    {
        _sqlConnection?.Close();
        _sqlConnection?.Dispose();
    }
}