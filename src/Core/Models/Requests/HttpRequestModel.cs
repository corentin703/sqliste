namespace Sqliste.Core.Models.Requests;

public class HttpRequestModel
{
    public HttpMethod Method { get; set; } = HttpMethod.Get;
    public string Path { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public Dictionary<string, string> Cookies { get; set; } = new();
}