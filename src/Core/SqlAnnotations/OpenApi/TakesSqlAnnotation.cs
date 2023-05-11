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

    public TakesSqlAnnotation(string type, bool required, string description)
    {
        Type = type;
        Required = required;
        Description = description;
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Type);
    }
}