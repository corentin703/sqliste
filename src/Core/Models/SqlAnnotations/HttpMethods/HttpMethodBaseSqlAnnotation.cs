using Sqliste.Core.Contracts;

namespace Sqliste.Core.Models.SqlAnnotations.HttpMethods;

public abstract class HttpMethodBaseSqlAnnotation : ISqlAnnotation
{
    public string? Id { get; set; }
    
    public abstract HttpMethod Method { get; }

    protected HttpMethodBaseSqlAnnotation()
    {
        //
    }

    protected HttpMethodBaseSqlAnnotation(string? id)
    {
        Id = id;
    }

    public bool IsValid()
    {
        return true;
    }
}