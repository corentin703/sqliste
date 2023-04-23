using System.Net;

namespace Sqliste.Database.SqlServer.Models;

public class SqlServerHttpResponseModel
{
    public string? Body { get; set; }
    public HttpStatusCode? Status { get; set; }
    public string? Headers { get; set; }
}