namespace Sqliste.Core.Models.Http;

public class HttpOperationModel
{
    public string? Id { get; set; }
    public HttpMethod Method { get; set; } = HttpMethod.Post;
}