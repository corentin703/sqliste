using Microsoft.OpenApi.Models;

namespace Sqliste.Core.Models.Http;

public class HttpUriParam
{
    public string Name { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public ParameterLocation Location { get; set; } = ParameterLocation.Query;
}