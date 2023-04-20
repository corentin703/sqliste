namespace Sqliste.Core.Exceptions.SqlAnnotations;

public class SqlAnnotationPropertyNotFoundException : InvalidOperationException
{
    public SqlAnnotationPropertyNotFoundException(string? propertyName)
        : base($"{propertyName} is unknown")
    {
        //
    }
}