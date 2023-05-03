namespace Sqliste.Core.Models.Sql;

public class SqlErrorModel
{
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, object>? Attributes { get; set; }
    public Exception? RawException { get; set; }
}