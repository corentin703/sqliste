using System.Net;
using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations.OpenApi;

public class ProducesResponseTypeSqlAnnotation : ISqlAnnotation
{
    public HttpStatusCode StatusCode { get; set; }
    public string Description { get; set; } = string.Empty;

    public ProducesResponseTypeSqlAnnotation()
    {
        //
    }

    public ProducesResponseTypeSqlAnnotation(HttpStatusCode statusCode, string? description = null)
    {
        StatusCode = statusCode;
        Description = description ?? string.Empty;
    }

    public bool IsValid()
    {
        return true;
    }
}