namespace Sqliste.Core.SqlAnnotations.SqlAnnotations;

public class SqlAnnotationPropertyNotFoundException : InvalidOperationException
{
    public SqlAnnotationPropertyNotFoundException(string? propertyName) 
        : base($"{propertyName} is unknown")
    {
        //
    }
}