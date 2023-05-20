using System.Text.Json.Serialization;

namespace Sqliste.Core.Models.Http.FormData;

public class FormDataString : FormDataItem
{
    [JsonPropertyName("value")]
    public string? Value { get; }
    
    
    public FormDataString(string name, string? value) : base(name)
    {
        Value = value;
    }
}