using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Database;
using Sqliste.Database.SqlServer.Configuration;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerMigrationService : IDatabaseMigrationService
{
    private readonly IOptionsSnapshot<SqlServerConfiguration> _configuration;
    private readonly ILogger<SqlServerMigrationService> _logger;

    public SqlServerMigrationService(IOptionsSnapshot<SqlServerConfiguration> configuration, ILogger<SqlServerMigrationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void Migrate()
    {
        using SqlConnection connection = new(_configuration.Value.ConnectionString);
        Evolve evolve = new(connection, message => _logger.LogInformation(message))
        {
            Locations = new[] { "Database/SqlServer/Migrations", },
            IsEraseDisabled = true,
        };

        evolve.Migrate();
    }
}