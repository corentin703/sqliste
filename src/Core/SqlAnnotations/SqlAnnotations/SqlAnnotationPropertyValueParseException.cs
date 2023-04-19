namespace Sqliste.Core.SqlAnnotations.SqlAnnotations;

public class SqlAnnotationPropertyValueParseException : InvalidOperationException
{
    public SqlAnnotationPropertyValueParseException(string value) 
        : base($"Unable to parse {value}")
    {
        //
    }

    public SqlAnnotationPropertyValueParseException(string propertyName, string value) 
        : base($"Unable to parse {value} for {propertyName}")
    {
        //
    }
}