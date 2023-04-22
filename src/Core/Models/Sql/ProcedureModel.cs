using Sqliste.Core.Contracts;

namespace Sqliste.Core.Models.Sql;

public class ProcedureModel
{
    public string RoutePattern { get; set; } = string.Empty;
    public string Schema { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public HttpMethod[] HttpMethods { get; set; } = new [] { HttpMethod.Post, };

    public List<ArgumentModel> Arguments { get; set; } = new();

    public List<ISqlAnnotation> Annotations { get; set; } = new();
    public List<string> RouteParamNames { get; set; } = new();
}