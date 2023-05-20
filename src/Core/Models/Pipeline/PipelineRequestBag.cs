using System.Text.Json.Serialization;
using Sqliste.Core.Models.Http.FormData;

namespace Sqliste.Core.Models.Pipeline;

public class PipelineRequestBag
{
    [JsonPropertyName("body")]
    public string? Body { get; init; }
    
    [JsonPropertyName("cookies")]
    public string? Cookies { get; init; }
    
    [JsonPropertyName("headers")]
    public string? Headers { get; init; }
    
    [JsonPropertyName("contentType")]
    public string? ContentType { get; init; }

    [JsonPropertyName("pathParams")]
    public Dictionary<string, string> PathParams { get; init; } = new();
    
    [JsonPropertyName("queryParams")]
    public Dictionary<string, string> QueryParams { get; init; } = new();
    
    [JsonPropertyName("method")]
    public HttpMethod Method { get; init; } = HttpMethod.Get;
    
    [JsonPropertyName("path")]
    public string Path { get; init; } = string.Empty;
    
    [JsonPropertyName("queryString")]
    public string? QueryString { get; init; }

    public Dictionary<string, FormDataItem>? FormData { get; init; }
}