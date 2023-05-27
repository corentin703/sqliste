using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqliste.Core.Configuration;

namespace Sqliste.Database.Common.Extensions.ServiceCollection;

public static class AddDatabaseExtensions
{
    public static IServiceCollection AddDatabaseCommons(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfiguration>(configuration.GetSection("Database"));

        return services;
    }
}