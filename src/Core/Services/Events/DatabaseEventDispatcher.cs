using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Attributes.Events;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Models.Events;
using Sqliste.Core.Utils.Events;

namespace Sqliste.Core.Services.Events;

public class DatabaseEventDispatcher : IDatabaseEventDispatcher
{
    private readonly ILogger<DatabaseEventDispatcher> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<string, Type> _handlersByEventName;

    //private const string HandlersByEventNameCacheKey =
    //    $"{nameof(DatabaseEventDispatcher)}_{nameof(_handlersByEventName)}";

    public DatabaseEventDispatcher(ILogger<DatabaseEventDispatcher> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        //Dictionary<string, Type>? handlersByEventName;
        //if (!memoryCache.TryGetValue(HandlersByEventNameCacheKey, out handlersByEventName) || handlersByEventName == null)
        //{
        //    handlersByEventName = new Dictionary<string, Type>();
        //    foreach (Type type in GetType().Assembly.GetTypes())
        //    {
        //        SystemEventHandlerAttribute? attribute = type.GetCustomAttribute<SystemEventHandlerAttribute>();
        //        if (attribute == null)
        //            continue;

        //        if (!handlersByEventName.TryAdd(attribute.Name, type))
        //            _logger.LogWarning("{handlerName} is a duplicate : ignoring", type.FullName ?? type.Name);
        //    }

        //    memoryCache.Set(HandlersByEventNameCacheKey, handlersByEventName);
        //}

        _handlersByEventName = EventHandlersUtils.HandlersByEventName;
    }

    public async Task DispatchEventsAsync(List<EventModel> events)
    {
        foreach (EventModel eventModel in events)
        {
            if (eventModel.Type == "SYS")
                await DispatchSystemEvent(eventModel);
            else 
                await DispatchCustomEvent(eventModel);
        }
    }

    private async Task DispatchSystemEvent(EventModel model)
    {
        Type? handlerType = null;
        if (!_handlersByEventName.TryGetValue(model.Name, out handlerType))
        {
            _logger.LogWarning("Corresponding event handler type not found for SYS event name {eventName}", model.Name);
            return;
        }

        IDatabaseEventHandler? eventHandler = _serviceProvider.GetService(handlerType) as IDatabaseEventHandler;
        if (eventHandler == null)
        {
            _logger.LogWarning(
                "Corresponding event handler type not registered in service provider for SYS event name {eventName} (event handler type name : {typeName}",
                model.Name, 
                handlerType.FullName ?? handlerType.Name
            );
            return;
        }

        await eventHandler.Handle(model);
    }

    private async Task DispatchCustomEvent(EventModel model)
    {

    }
}