using Sqliste.Core.Contracts;

namespace Sqliste.Core.Models.SqlAnnotations;

public record RouteSqlAnnotation : ISqlAnnotation
{
    public RouteSqlAnnotation()
    {
        //
    }

    public RouteSqlAnnotation(string path)
    {
        Path = path;
    }

    public string Path { get; set; } = "/";

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Path);
    }
}