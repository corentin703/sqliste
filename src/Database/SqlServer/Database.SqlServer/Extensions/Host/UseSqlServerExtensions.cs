using Coravel;
using Microsoft.Extensions.Hosting;
using Sqliste.Database.SqlServer.Jobs.Scheduling;

namespace Sqliste.Database.SqlServer.Extensions.Host;

public static class UseSqlServerExtensions
{
    public static IHost UseSqlServer(this IHost host)
    {
        host.Services.UseScheduler(scheduler =>
        {
            scheduler.Schedule<DatabaseAppEventCleaningInvocable>().Hourly();
        });

        return host;
    }
}