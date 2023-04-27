using DapperCodeFirstMappings.Attributes;

namespace Sqliste.Core.Models.Events;

[DapperEntity]
public class EventModel
{
    [DapperColumn("id")]
    public long Id { get; init; }

    [DapperColumn("type")]
    public string Type { get; init; } = string.Empty;

    [DapperColumn("name")]
    public string Name { get; init; } = string.Empty;

    [DapperColumn("args")]
    public string? Args { get; init; }

    [DapperColumn("inserted_at")]
    public DateTime InsertedAt { get; init; }
}