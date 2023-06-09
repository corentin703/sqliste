using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;

namespace Sqliste.Infrastructure.Extensions.Host;

public static class IntrospectionExtensions
{
    public static async Task RunInitialIntrospectionAsync(this IHost host)
    {
        ILogger logger = host.Services.GetRequiredService<ILogger>();

        await using AsyncServiceScope scope = host.Services.CreateAsyncScope();
        IIntrospectionService introspectionService =
            scope.ServiceProvider.GetRequiredService<IIntrospectionService>();
        
        await introspectionService.IntrospectAsync();
            
        logger.LogInformation("Initial introspection run with success");
    }
}