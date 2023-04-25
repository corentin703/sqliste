using Sqliste.Core.Models.Events;

namespace Sqliste.Core.Contracts.Services.Events;

public interface IDatabaseEventDispatcher
{
    public Task DispatchEventsAsync(List<EventModel> events);
}