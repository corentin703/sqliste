using System.Net;

namespace Sqliste.Core.Models.Http;

public class HttpResponseModel
{
    public object? Body { get; set; }
    public HttpStatusCode? Status { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
}