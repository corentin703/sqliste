using System.Net;
using Sqliste.Core.Contracts;

namespace Sqliste.Core.SqlAnnotations.OpenApi;

public class AcceptsSqlAnnotation : ISqlAnnotation
{
    public string Mime { get; set; } = string.Empty;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Mime);
    }
}