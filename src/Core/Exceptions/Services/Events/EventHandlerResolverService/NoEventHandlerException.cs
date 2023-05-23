namespace Sqliste.Core.Exceptions.Services.Events.EventHandlerResolverService;

public class NoEventHandlerException : InvalidOperationException
{
    public NoEventHandlerException() : base("No handler available for this event")
    {
        //
    }
}