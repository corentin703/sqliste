using Sqliste.Server.Cli;
using Sqliste.Server.Cli.Handlers;

namespace Sqliste.Server.Extensions.CliExtensions;

public static class CliExtensions
{
    public static IServiceCollection AddCliServices(this IServiceCollection services)
    {
        services.AddScoped<CliApplication>();
        services.AddScoped<SwaggerCommandHandler>();
        services.AddScoped<DatabaseMigrationCommandHandler>();
        
        return services;
    }
}