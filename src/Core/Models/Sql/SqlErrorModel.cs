using System.Text.Json.Serialization;

namespace Sqliste.Core.Models.Sql;

public class SqlErrorModel
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    
    [JsonPropertyName("attributes")]
    public Dictionary<string, object>? Attributes { get; set; }
    
    [JsonIgnore]
    public Exception? RawException { get; set; }
}