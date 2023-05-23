using Sqliste.Core.Attributes.Events;
using System.Reflection;

namespace Sqliste.Core.Utils.Events;

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