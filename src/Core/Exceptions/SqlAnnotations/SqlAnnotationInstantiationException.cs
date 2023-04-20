namespace Sqliste.Core.Exceptions.SqlAnnotations;

public class SqlAnnotationInstantiationException : InvalidOperationException
{
    public SqlAnnotationInstantiationException(string annotationName)
        : base($"Unable to instantiate {annotationName}")
    {
        //
    }
}