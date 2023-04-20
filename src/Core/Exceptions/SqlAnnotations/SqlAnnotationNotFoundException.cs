namespace Sqliste.Core.Exceptions.SqlAnnotations;

public class SqlAnnotationNotFoundException : InvalidOperationException
{
    public SqlAnnotationNotFoundException(string annotationName)
        : base($"{annotationName} doesn't exists")
    {
        //
    }
}