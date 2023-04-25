using Sqliste.Core.Models.Events;

namespace Sqliste.Core.Contracts.Services.Events;

public interface IDatabaseEventHandler
{
    public Task Handle(EventModel model);
}