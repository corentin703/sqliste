namespace Sqliste.Core.Exceptions.SqlAnnotations;

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