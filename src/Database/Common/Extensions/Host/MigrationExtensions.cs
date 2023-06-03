// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using Sqliste.Core.Configuration;
// using Sqliste.Database.Common.Contracts.Services;
//
// namespace Sqliste.Database.Common.Extensions.Host;
//
// public static class MigrationExtensions
// {
//     public static async Task<IHost> RunMigrationsAsync(this IHost host)
//     {
//         IOptions<DatabaseConfiguration> databaseConfiguration = host.Services.GetRequiredService<IOptions<DatabaseConfiguration>>();
//         ILogger logger = host.Services.GetRequiredService<ILogger>();
//
//         if (!databaseConfiguration.Value.Migration.Enable)
//         {
//             logger.LogWarning("Migration is disabled");
//             return host;
//         }
//
//         await using AsyncServiceScope scope = host.Services.CreateAsyncScope();
//         IDatabaseMigrationService databaseMigrationService = 
//             scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
//         
//         databaseMigrationService.Migrate();
//
//         logger.LogInformation("Database migrated with success");
//
//         return host;
//     }
// }