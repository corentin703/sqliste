using System.Net;
using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations.OpenApi;

public class AcceptSqlAnnotation : ISqlAnnotation
{
    public HttpStatusCode StatusCode { get; set; }

    public AcceptSqlAnnotation()
    {
        //
    }

    public AcceptSqlAnnotation(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public bool IsValid()
    {
        return true;
    }
}