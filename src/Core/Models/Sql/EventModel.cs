namespace Sqliste.Core.Models.Sql;

public class EventModel
{
    public long Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Args { get; set; } = string.Empty;
    public DateTime InsertedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}