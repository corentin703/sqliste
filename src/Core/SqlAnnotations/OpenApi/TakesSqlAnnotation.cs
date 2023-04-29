using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations.OpenApi;

public class TakesSqlAnnotation : ISqlAnnotation
{
    public bool Required { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public TakesSqlAnnotation()
    {
        //
    }

    public TakesSqlAnnotation(bool required, string type, string description)
    {
        Required = required;
        Description = description;
        Type = type;
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Type);
    }
}