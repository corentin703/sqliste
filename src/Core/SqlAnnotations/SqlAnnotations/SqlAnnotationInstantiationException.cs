namespace Sqliste.Core.SqlAnnotations.SqlAnnotations;

public class SqlAnnotationInstantiationException : InvalidOperationException
{
    public SqlAnnotationInstantiationException(string annotationName) 
        : base($"Unable to instantiate {annotationName}")
    {
        //
    }
}