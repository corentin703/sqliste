using Sqliste.Core.Contracts;

namespace Sqliste.Core.Models;

public class ProcedureModel
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<ArgumentModel> Arguments { get; set; } = new();

    public List<ISqlAnnotation> Annotations { get; set; } = new();
}