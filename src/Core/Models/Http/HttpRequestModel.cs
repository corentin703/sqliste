using DapperCodeFirstMappings.Attributes;
using System.Net;
using Sqliste.Core.Constants;
using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Models.Http;

[DapperEntity]
public class HttpRequestModel
{
    [DapperColumn(SystemQueryParametersConstants.RequestBody)]
    public string? RequestBody { get; set; }
    
    [DapperColumn(SystemQueryParametersConstants.ResponseBody)]
    public string? ResponseBody { get; set; }

    [DapperColumn(SystemQueryParametersConstants.RequestCookies)]
    public string? RequestCookies { get; set; }

    [DapperColumn(SystemQueryParametersConstants.ResponseCookies)]
    public string? ResponseCookies { get; set; }

    [DapperColumn(SystemQueryParametersConstants.DataBag)]
    public string? DataBag { get; set; }

    [DapperColumn(SystemQueryParametersConstants.RequestHeaders)]
    public string? RequestHeaders { get; set; }

    [DapperColumn(SystemQueryParametersConstants.ResponseHeaders)]
    public string? ResponseHeaders { get; set; }

    [DapperColumn(SystemQueryParametersConstants.Next)]
    public bool Next { get; set; } = true;

    [DapperColumn(SystemQueryParametersConstants.Status)]
    public HttpStatusCode? Status { get; set; }
    
    [DapperColumn(SystemQueryParametersConstants.Session)]
    public string? Session { get; set; }

    public Dictionary<string, string> PathParams { get; set; } = new();
    public Dictionary<string, string> QueryParams { get; set; } = new();
    public HttpMethod Method { get; set; } = HttpMethod.Get;
    public string Path { get; set; } = string.Empty;
    public string? QueryString { get; set; }
    public string? ContentType { get; set; }
    public SqlErrorModel? Error { get; set; }
}