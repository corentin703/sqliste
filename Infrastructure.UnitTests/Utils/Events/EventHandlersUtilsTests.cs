using FluentAssertions;
using Sqliste.Infrastructure.Services.Events;
using Sqliste.Infrastructure.Utils.Events;

namespace Infrastructure.UnitTests.Utils.Events;

public class EventHandlersUtilsTests
{
    [Theory]
    [InlineData("WebSchemaUpdate", typeof(WebSchemaUpdateSystemDatabaseEventHandler))]
    public void TestInstantiation(string eventName, Type eventHandlerType)
    {
        Dictionary<string, Type> handlers = EventHandlersUtils.GetHandlersByEventName();
        
        handlers.Should().ContainKey(eventName);
        handlers[eventName].Should().Be(eventHandlerType);
    }
}