using System.Net;

namespace Sqliste.Core.Models.Http;

public class HttpResponseModel
{
    public object? Data { get; set; }
    public HttpStatusCode Status { get; set; } = HttpStatusCode.NoContent;
}