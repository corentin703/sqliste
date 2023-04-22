using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations.HttpMethods;

public abstract class HttpMethodBaseSqlAnnotation : ISqlAnnotation
{
    public bool IsValid()
    {
        return true;
    }
}