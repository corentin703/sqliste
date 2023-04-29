using DapperCodeFirstMappings.Attributes;

namespace Sqliste.Core.Models;

[DapperEntity]
public class OpenApiTypeGetResponseModel
{
    [DapperColumn("type")]
    public string Type { get; set; } = string.Empty;

    [DapperColumn("format")]
    public string Format { get; set; } = string.Empty;
}