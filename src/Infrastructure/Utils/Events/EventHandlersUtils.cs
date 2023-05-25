using System.Reflection;
using Sqliste.Core.Attributes.Events;

namespace Sqliste.Infrastructure.Utils.Events;

internal static class EventHandlersUtils
{
    public static Dictionary<string, Type> GetHandlersByEventName()
    {
        Dictionary<string, Type> handlersByEventName = new();

        foreach (Type type in typeof(EventHandlersUtils).Assembly.GetTypes())
        {
            SystemEventHandlerAttribute? attribute = type.GetCustomAttribute<SystemEventHandlerAttribute>();
            if (attribute == null)
                continue;

            try
            {
                handlersByEventName.Add(attribute.Name, type);
            }
            catch
            {
                throw new InvalidOperationException($"{type.FullName ?? type.Name} is a duplicate : ignoring");
            }
        }

        return handlersByEventName;
    }
}