using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations.HttpMethods;

public abstract class HttpMethodBaseSqlAnnotation : ISqlAnnotation
{
    public string? Id { get; set; }

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