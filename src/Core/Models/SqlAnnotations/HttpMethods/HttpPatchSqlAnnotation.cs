namespace Sqliste.Core.Models.SqlAnnotations.HttpMethods;

public class HttpPatchSqlAnnotation : HttpMethodBaseSqlAnnotation
{
    public override HttpMethod Method { get; } = HttpMethod.Patch;
    
    public HttpPatchSqlAnnotation()
    {
        //
    }

    public HttpPatchSqlAnnotation(string? id) : base(id)
    {
        //
    }
}