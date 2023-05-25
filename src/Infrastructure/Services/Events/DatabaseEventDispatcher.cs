using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Exceptions.Services.Events.EventHandlerResolverService;
using Sqliste.Core.Models.Events;

namespace Sqliste.Infrastructure.Services.Events;

internal class DatabaseEventDispatcher : IDatabaseEventDispatcher
{
    private readonly ILogger<DatabaseEventDispatcher> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDatabaseEventHandlerResolver _databaseEventHandlerResolver;

    public DatabaseEventDispatcher(ILogger<DatabaseEventDispatcher> logger, IServiceProvider serviceProvider, IDatabaseEventHandlerResolver databaseEventHandlerResolver)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _databaseEventHandlerResolver = databaseEventHandlerResolver;
    }

    public async Task DispatchEventsAsync(List<EventModel> events)
    {
        foreach (EventModel eventModel in events)
        {
            if (eventModel.Type == "SYS")
                await DispatchSystemEvent(eventModel);
            else 
                await DispatchPluginEvent(eventModel);
        }
    }

    private async Task DispatchSystemEvent(EventModel model)
    {
        try
        {
            Type handlerType = _databaseEventHandlerResolver.GetHandlerByEventName(model.Name);

            IDatabaseEventHandler? eventHandler = _serviceProvider.GetService(handlerType) as IDatabaseEventHandler;
            if (eventHandler == null)
            {
                _logger.LogWarning(
                    "Corresponding event handler type not registered in service provider for SYS event name {EventName} (event handler type name : {TypeName}",
                    model.Name,
                    handlerType.FullName ?? handlerType.Name
                );
                return;
            }

            await eventHandler.Handle(model);
        }
        catch (NoEventHandlerException noEventHandlerException)
        {
            _logger.LogWarning(
                exception: noEventHandlerException, 
                "Corresponding event handler type not found for SYS event name {EventName}",
                model.Name
            );
        }
    }

    private Task DispatchPluginEvent(EventModel model)
    {
        _logger.LogError("Custom event dispatching not implemented");
        return Task.CompletedTask;
    }
}