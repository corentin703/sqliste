using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations;

public class HttpDeleteSqlAnnotation : ISqlAnnotation
{
    public bool IsValid()
    {
        return true;
    }
}