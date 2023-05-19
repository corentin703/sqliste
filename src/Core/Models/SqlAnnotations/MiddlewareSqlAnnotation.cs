using Sqliste.Core.Contracts;

namespace Sqliste.Core.Models.SqlAnnotations;

public class MiddlewareSqlAnnotation : ISqlAnnotation
{
    public int Order { get; set; } = 1;
    public string PathStarts { get; set; } = "/";
    public bool After { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(PathStarts);
    }
}