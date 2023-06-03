using Sqliste.Database.Common.Contracts.Services;

namespace Sqliste.Server.Cli.Handlers;

public class DatabaseMigrationCommandHandler
{
    private readonly IDatabaseMigrationService _migrationService;
    private readonly ILogger<DatabaseMigrationCommandHandler> _logger;

    public DatabaseMigrationCommandHandler(IDatabaseMigrationService migrationService, ILogger<DatabaseMigrationCommandHandler> logger)
    {
        _migrationService = migrationService;
        _logger = logger;
    }

    public async Task HandleAsync()
    {
        _migrationService.Migrate();
        _logger.LogInformation("Database migrated with success");
    }
}