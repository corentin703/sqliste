using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;

namespace Sqliste.Core.Extensions.Host;

public static class IntrospectionExtensions
{
    public static async Task<IHost> RunInitialIntrospectionAsync(this IHost host)
    {
        ILogger logger = host.Services.GetRequiredService<ILogger>();

        await using AsyncServiceScope scope = host.Services.CreateAsyncScope();
        ISqlisteIntrospectionService sqlisteIntrospectionService =
            scope.ServiceProvider.GetRequiredService<ISqlisteIntrospectionService>();
        
        await sqlisteIntrospectionService.IntrospectAsync();
            
        logger.LogInformation("Initial introspection run with success");

        return host;
    }
}