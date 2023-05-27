namespace Sqliste.Core.Constants;

public static class SystemQueryParametersConstants
{
    #region Request

    public const string RequestModel = "request_model";
    public const string RequestContentType = "request_content_type";
    public const string RequestBody = "request_body";
    public const string RequestFormData = "request_form_data";
    public const string RequestHeaders = "request_headers";
    public const string RequestCookies = "request_cookies";
    public const string RequestPath = "request_path";
    public const string RequestMethod = "request_method";
    public const string QueryParams = "query_params";
    public const string PathParams = "path_params";

    public const string RequestFormFilePrefix = "request_form_file_";

    #endregion

    #region Response

    public const string ResponseBody = "response_body";
    public const string ResponseFile = "response_file";
    public const string ResponseFileName = "response_file_name";
    public const string ResponseFileInline = "response_file_inline";
    public const string ResponseContentType = "response_content_type";
    public const string ResponseCookies = "response_cookies";
    public const string ResponseHeaders = "response_headers";
    public const string ResponseStatus = "response_status";
    public const string ResponseModel = "response_model";

    #endregion

    #region Storage

    public const string PipelineStorage = "pipeline_storage";
    public const string Session = "session";

    #endregion

    #region Middlewares

    public const string Next = "next";
    public const string Error = "error";

    #endregion
}