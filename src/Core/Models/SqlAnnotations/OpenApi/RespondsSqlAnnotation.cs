using System.Net;
using Sqliste.Core.Contracts;

namespace Sqliste.Core.Models.SqlAnnotations.OpenApi;

public class RespondsSqlAnnotation : ISqlAnnotation
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

    public RespondsSqlAnnotation()
    {
        //
    }

    public RespondsSqlAnnotation(string type, HttpStatusCode status, string description)
    {
        Type = type;
        Status = status;
        Description = description;
    }

    public bool IsValid()
    {
        return true;
    }
}