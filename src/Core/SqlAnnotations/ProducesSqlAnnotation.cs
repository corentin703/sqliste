using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations;

public class ProducesSqlAnnotation : ISqlAnnotation
{
    public string Mime { get; set; } = string.Empty;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Mime);
    }
}