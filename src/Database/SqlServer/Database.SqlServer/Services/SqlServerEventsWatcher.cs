using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Database.SqlServer.Configuration;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerEventsWatcher : IDisposable, IDatabaseEventWatcher
{
    private SqlConnection? _sqlConnection;
    private SqlDependency? _sqlDependency;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SqlServerEventsWatcher> _logger;
    private readonly IOptionsMonitor<SqlServerConfiguration> _configurationMonitor;

    public SqlServerEventsWatcher(IServiceScopeFactory scopeFactory,
        ILogger<SqlServerEventsWatcher> logger, IOptionsMonitor<SqlServerConfiguration> configurationMonitor)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configurationMonitor = configurationMonitor;
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
        if (sqlNotificationInfo.Equals(SqlNotificationInfo.Insert) || sqlNotificationInfo.Equals(SqlNotificationInfo.Update))
            NotifyEventsAsync().GetAwaiter().GetResult();

        SetupDependency();
    }

    private void SetupDependency()
    {
        if (_sqlConnection == null)
            return;

        string query = "SELECT [Id], [Type], [Args], [InsertedAt], [ProcessedAt] FROM [sqliste].[app_events] WHERE [ProcessedAt] IS NULL";
        using SqlCommand sqlCommand = new(query, _sqlConnection);
        _sqlDependency = new SqlDependency(sqlCommand);
        _sqlDependency.OnChange += OnDependencyChange;
        
        using SqlDataReader reader = sqlCommand.ExecuteReader();
    }

    private async Task NotifyEventsAsync()
    {
        _logger.LogInformation("Detected change in web schema's procedures");
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();

        IDatabaseIntrospectionService databaseIntrospectionService
            = scope.ServiceProvider.GetRequiredService<IDatabaseIntrospectionService>();

        databaseIntrospectionService.Clear();
        await databaseIntrospectionService.IntrospectAsync();
        _logger.LogInformation("Introspection done");
    }

    public void Dispose()
    {
        _sqlConnection?.Close();
        _sqlConnection?.Dispose();
    }
}