using System.Text.Json;

namespace Sqliste.Core.Models.Http;

public class HttpRequestModel
{
    public HttpMethod Method { get; set; } = HttpMethod.Get;
    public string Path { get; set; } = string.Empty;
    public string? QueryString { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public Dictionary<string, string> Cookies { get; set; } = new();
    public string? Body { get; set; }
}