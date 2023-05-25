using Microsoft.Extensions.DependencyInjection;
using Sqliste.Infrastructure.Utils.Events;

namespace Sqliste.Infrastructure.Extensions.ServiceCollection;

internal static class AddDatabaseEventHandlersExtensions
{
    public static IServiceCollection AddDatabaseEventHandlers(this IServiceCollection services)
    {
        foreach (Type type in EventHandlersUtils.GetHandlersByEventName().Values)
        {
            services.AddTransient(type);
        }

        return services;
    }
}