using FluentAssertions;
using Sqliste.Core.Services.Events;
using Sqliste.Core.Utils.Events;

namespace Core.UnitTests.Utils.Events;

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