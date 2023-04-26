using Sqliste.Core.Utils.Events;

namespace Sqliste.Server.Extensions.ServiceCollection;

public static class AddDatabaseEventHandlersExtensions
{
    public static IServiceCollection AddDatabaseEventHandlers(this IServiceCollection services)
    {
        foreach (Type type in EventHandlersUtils.HandlersByEventName.Values)
        {
            services.AddTransient(type);
        }

        return services;
    }
}