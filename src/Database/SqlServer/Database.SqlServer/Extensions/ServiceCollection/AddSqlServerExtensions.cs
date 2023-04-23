using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqliste.Core.Contracts.Services;
using Sqliste.Database.SqlServer.Configuration;
using Sqliste.Database.SqlServer.Services;

namespace Sqliste.Database.SqlServer.Extensions.ServiceCollection;

public static class AddSqlServerExtensions
{
    public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlServerConfiguration>(configuration.GetSection("Database"));

        services.AddScoped<IDatabaseService, SqlServerDatabaseService>();
        services.AddScoped<IDatabaseMigrationService, SqlServerMigrationService>();
        services.AddScoped<IDatabaseIntrospectionService, SqlServerIntrospectionService>();
        services.AddScoped<IRequestHandlerService, SqlServerRequestHandlerService>();

        return services;
    }
}