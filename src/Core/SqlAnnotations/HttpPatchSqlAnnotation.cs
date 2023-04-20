using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations;

public class HttpPatchSqlAnnotation : ISqlAnnotation
{
    public bool IsValid()
    {
        return true;
    }
}