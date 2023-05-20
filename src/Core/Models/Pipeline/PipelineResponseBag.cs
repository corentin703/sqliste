using System.Net;
using DapperCodeFirstMappings.Attributes;
using Sqliste.Core.Constants;
using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Models.Pipeline;

[DapperEntity]
public class PipelineResponseBag
{
    [DapperColumn(SystemQueryParametersConstants.ResponseBody)]
    public string? Body { get; set; }
    
    [DapperColumn(SystemQueryParametersConstants.ResponseFile)]
    public byte[]? File { get; set; }

    [DapperColumn(SystemQueryParametersConstants.ResponseFileName)]
    public string? FileName { get; set; }

    [DapperColumn(SystemQueryParametersConstants.ResponseFileInline)]
    public bool FileInline { get; set; }

    [DapperColumn(SystemQueryParametersConstants.ResponseCookies)]
    public string? Cookies { get; set; }

    [DapperColumn(SystemQueryParametersConstants.ResponseHeaders)]
    public string? Headers { get; set; }

    [DapperColumn(SystemQueryParametersConstants.ResponseStatus)]
    public HttpStatusCode? Status { get; set; }
    
    [DapperColumn(SystemQueryParametersConstants.ResponseContentType)]
    public string? ContentType { get; set; }

    #region Middlewares

    [DapperColumn(SystemQueryParametersConstants.Next)]
    public bool Next { get; set; } = true;
    
    public SqlErrorModel? Error { get; set; }

    #endregion

    #region Storage

    [DapperColumn(SystemQueryParametersConstants.PipelineStorage)]
    public string PipelineStorage { get; set; } = "{}";

    [DapperColumn(SystemQueryParametersConstants.Session)]
    public string? Session { get; set; }

    #endregion
}