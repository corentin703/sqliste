using System.Text.Json.Serialization;

namespace Sqliste.Core.Models.Http.FormData;

[JsonDerivedType(typeof(FormDataFile))]
[JsonDerivedType(typeof(FormDataString))]
public abstract class FormDataItem
{
    [JsonPropertyName("name")]
    public string Name { get; }

    protected FormDataItem(string name)
    {
        Name = name;
    }
}