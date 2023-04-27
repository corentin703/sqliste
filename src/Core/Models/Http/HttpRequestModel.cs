using DapperCodeFirstMappings.Attributes;
using System.Net;
using Sqliste.Core.Constants;

namespace Sqliste.Core.Models.Http;

public class HttpRequestModel
{
    [DapperColumn(SystemQueryParametersConstants.Body)]
    public string? Body { get; set; }

    [DapperColumn(SystemQueryParametersConstants.Cookies)]
    public string? Cookies { get; set; }

    [DapperColumn(SystemQueryParametersConstants.DataBag)]
    public string? DataBag { get; set; }

    [DapperColumn(SystemQueryParametersConstants.Headers)]
    public string? Headers { get; set; }

    [DapperColumn(SystemQueryParametersConstants.Next)]
    public bool Next { get; set; } = true;

    public Dictionary<string, string> PathParams { get; set; } = new();
    public HttpMethod Method { get; set; } = HttpMethod.Get;
    public string Path { get; set; } = string.Empty;
    public string? QueryString { get; set; }
    public string? ContentType { get; set; }
    public HttpStatusCode? Status { get; set; }
}