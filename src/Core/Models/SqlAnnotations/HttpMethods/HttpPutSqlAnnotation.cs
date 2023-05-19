namespace Sqliste.Core.Models.SqlAnnotations.HttpMethods;

public class HttpPutSqlAnnotation : HttpMethodBaseSqlAnnotation
{
    public override HttpMethod Method { get; } = HttpMethod.Put;
    
    public HttpPutSqlAnnotation()
    {
        //
    }

    public HttpPutSqlAnnotation(string? id) : base(id)
    {
        //
    }
}