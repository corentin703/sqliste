using FluentAssertions;
using Sqliste.Core.Contracts;
using Sqliste.Core.Services.Events;
using Sqliste.Core.Utils.Events;
using Sqliste.Core.Utils.SqlAnnotations;

namespace Core.UnitTests.Utils.Events;

public class EventHandlersUtilsTests
{
    [Theory]
    [InlineData("WebSchemaUpdate", typeof(WebSchemaUpdateSystemDatabaseEventHandler))]
    public void TestInstantiation(string eventName, Type eventHandlerType)
    {
        EventHandlersUtils.HandlersByEventName.Should().ContainKey(eventName);
        EventHandlersUtils.HandlersByEventName[eventName].Should().Be(eventHandlerType);
    }
}