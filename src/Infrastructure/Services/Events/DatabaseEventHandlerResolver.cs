using Microsoft.Extensions.Caching.Memory;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Exceptions.Services.Events.EventHandlerResolverService;
using Sqliste.Infrastructure.Utils.Events;

namespace Sqliste.Infrastructure.Services.Events;

internal class DatabaseEventHandlerResolver : IDatabaseEventHandlerResolver
{
    private IMemoryCache _memoryCache;
    private const string HandlersByEventNameCacheKey = $"{nameof(DatabaseEventHandlerResolver)}_HandlersByEventName";

    public DatabaseEventHandlerResolver(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Dictionary<string, Type> GetHandlers()
    {
        Dictionary<string, Type>? handlersByEventName;
        if (_memoryCache.TryGetValue(HandlersByEventNameCacheKey, out handlersByEventName) && handlersByEventName != null)
            return handlersByEventName;

        handlersByEventName = EventHandlersUtils.GetHandlersByEventName();
        _memoryCache.Set(HandlersByEventNameCacheKey, handlersByEventName);
        return handlersByEventName;
    }

    public Type GetHandlerByEventName(string eventName)
    {
        Type? handlerType;
        if (!GetHandlers().TryGetValue(eventName, out handlerType))
            throw new NoEventHandlerException();

        return handlerType;
    }
}