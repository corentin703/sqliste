using System.Net;
using DapperCodeFirstMappings.Attributes;
using Sqliste.Core.Constants;
using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Models.Pipeline;

[DapperEntity]
public class PipelineResponseBag
{
    [DapperColumn("body")]
    public string? Body { get; set; }

    [DapperColumn("cookies")]
    public string? Cookies { get; set; }

    [DapperColumn("headers")]
    public string? Headers { get; set; }

    [DapperColumn("status")]
    public HttpStatusCode? Status { get; set; }
    
    [DapperColumn("content_type")]
    public string? ContentType { get; set; }

    #region Middlewares

    [DapperColumn(SystemQueryParametersConstants.Next)]
    public bool Next { get; set; } = true;
    
    public SqlErrorModel? Error { get; set; }

    #endregion

    #region DataBag

    [DapperColumn(SystemQueryParametersConstants.RequestStorage)]
    public string RequestStorage { get; set; } = "{}";

    [DapperColumn(SystemQueryParametersConstants.Session)]
    public string? Session { get; set; }

    #endregion
}