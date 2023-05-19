namespace Sqliste.Core.SqlAnnotations.HttpMethods;

public class HttpGetSqlAnnotation : HttpMethodBaseSqlAnnotation
{
    public override HttpMethod Method { get; } = HttpMethod.Get;
    
    public HttpGetSqlAnnotation()
    {
        //
    }

    public HttpGetSqlAnnotation(string? id) : base(id)
    {
        //
    }
}