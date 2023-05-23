namespace Sqliste.Core.Contracts.Services.Events;

public interface IDatabaseEventHandlerResolver
{
    public Dictionary<string, Type> GetHandlers();
    public Type GetHandlerByEventName(string eventName);
}