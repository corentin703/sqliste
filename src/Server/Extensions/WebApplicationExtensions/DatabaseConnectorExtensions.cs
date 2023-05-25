using Sqliste.Database.SqlServer.Extensions.Host;
using Sqliste.Database.SqlServer.Extensions.ServiceCollection;
using Sqliste.Server.Exceptions.DatabaseConnectors;

namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class DatabaseConnectorExtensions
{
    private const string ConnectorConfigKey = "Database:Connector";
    
    public static WebApplicationBuilder AddDatabaseConnector(this WebApplicationBuilder builder)
    {
        string? connectorType = builder.Configuration.GetValue<string>(ConnectorConfigKey);
        if (string.IsNullOrEmpty(connectorType))
            throw new NoDatabaseConnectorGivenException();

        switch (connectorType.ToLower())
        {
            case "sqlserver":
                builder.Services.AddSqlServer(builder.Configuration);
                break;
            default:
                throw new DatabaseConnectorNotFoundException();
        }
        
        return builder;
    }

    public static WebApplication UseDatabaseConnector(this WebApplication app)
    {
        string? connectorType = app.Configuration.GetValue<string>(ConnectorConfigKey);
        if (string.IsNullOrEmpty(connectorType))
            throw new NoDatabaseConnectorGivenException();

        switch (connectorType.ToLower())
        {
            case "sqlserver":
                app.UseSqlServer();
                break;
            default:
                throw new DatabaseConnectorNotFoundException();
        }
        
        return app;
    }
}