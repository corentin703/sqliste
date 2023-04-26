using System.Net.NetworkInformation;
using Sqliste.Core.Attributes.Events;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Sqliste.Core.Utils.Events;

public static class EventHandlersUtils
{
    private static Dictionary<string, Type>? _handlersByEventName;

    public static Dictionary<string, Type> HandlersByEventName
    {
        get
        {
            //if (_handlersByEventName != null) 
            //    return _handlersByEventName;

            _handlersByEventName = new Dictionary<string, Type>();

            foreach (Type type in typeof(EventHandlersUtils).Assembly.GetTypes())
            {
                SystemEventHandlerAttribute? attribute = type.GetCustomAttribute<SystemEventHandlerAttribute>();
                if (attribute == null)
                    continue;

                try
                {
                    _handlersByEventName.Add(attribute.Name, type);
                }
                catch
                {
                    throw new InvalidOperationException($"{type.FullName ?? type.Name} is a duplicate : ignoring");
                }
            }

            return _handlersByEventName;
        }
    }
}