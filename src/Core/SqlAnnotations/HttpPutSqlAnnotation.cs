using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations;

public class HttpPutSqlAnnotation : ISqlAnnotation
{
    public bool IsValid()
    {
        return true;
    }
}