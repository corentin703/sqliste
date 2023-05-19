namespace Sqliste.Core.Models.SqlAnnotations.HttpMethods;

public class HttpPostSqlAnnotation : HttpMethodBaseSqlAnnotation
{
    public override HttpMethod Method { get; } = HttpMethod.Post;
    
    public HttpPostSqlAnnotation()
    {
        //
    }

    public HttpPostSqlAnnotation(string? id) : base(id)
    {
        //
    }
}