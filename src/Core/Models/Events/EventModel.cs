namespace Sqliste.Core.Models.Events;

public class EventModel
{
    public long Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Args { get; init; }
    public DateTime InsertedAt { get; init; }
}