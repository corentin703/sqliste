using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations;

public class HttpPostSqlAnnotation : ISqlAnnotation
{
    public bool IsValid()
    {
        return true;
    }
}