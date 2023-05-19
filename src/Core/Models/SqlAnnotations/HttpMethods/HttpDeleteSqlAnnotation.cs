namespace Sqliste.Core.Models.SqlAnnotations.HttpMethods;

public class HttpDeleteSqlAnnotation : HttpMethodBaseSqlAnnotation
{
    public override HttpMethod Method { get; } = HttpMethod.Delete;

    public HttpDeleteSqlAnnotation()
    {
        //
    }

    public HttpDeleteSqlAnnotation(string? id) : base(id)
    {
        //
    }
}