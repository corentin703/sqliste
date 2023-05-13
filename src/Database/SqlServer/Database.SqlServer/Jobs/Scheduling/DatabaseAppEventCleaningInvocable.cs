using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Database;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Jobs.Scheduling;

public class DatabaseAppEventCleaningInvocable : IInvocable
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<DatabaseAppEventCleaningInvocable> _logger;

    public DatabaseAppEventCleaningInvocable(IDatabaseService databaseService, ILogger<DatabaseAppEventCleaningInvocable> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task Invoke()
    {
        _logger.LogInformation("Running app event table's cleaning procedure");
        (string query, object args) = MaintenanceSqlQueries.GetAppEventTableCleaningProcedure();
        await _databaseService.QueryAsync(query, args);
        _logger.LogInformation("App event table's cleaning procedure executed");
    }
}