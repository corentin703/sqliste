using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Sqliste.Core.Models.Http.FormData;

public class FormDataFile : FormDataItem
{
    [JsonIgnore]
    public byte[] Content { get; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; }
    
    [JsonPropertyName("length")]
    public long Length { get; }

    [JsonPropertyName("contentType")]
    public string ContentType { get; }

    [JsonPropertyName("contentDisposition")]
    public string ContentDisposition { get; }
    
    [JsonPropertyName("fileName")]
    public string FileName { get; }

    public FormDataFile(IFormFile file, byte[] content)
        : base(file.Name)
    {
        Content = content;
        Headers = file.Headers
            .ToDictionary(header => header.Key, header => header.Value.ToString());

        Length = file.Length;
        ContentType = file.ContentType;
        ContentDisposition = file.ContentDisposition;
        FileName = file.FileName;
    }
}