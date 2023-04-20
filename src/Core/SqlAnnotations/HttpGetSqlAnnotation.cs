using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations;

public class HttpGetSqlAnnotation : ISqlAnnotation
{
    public bool IsValid()
    {
        return true;
    }
}