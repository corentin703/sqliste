namespace Sqliste.Core.Attributes.Events;

[AttributeUsage(AttributeTargets.Class)]
public class SystemEventHandlerAttribute : Attribute
{
    public string Name { get; }

    public SystemEventHandlerAttribute(string name)
    {
        Name = name;
    }
}