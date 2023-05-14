namespace Sqliste.Core.Constants;

public static class SystemQueryParametersConstants
{
    #region Readonly

    public const string RequestBody = "request_body";
    public const string RequestCookies = "request_cookies";
    public const string QueryParams = "query_params";
    public const string PathParams = "path_params";
    public const string ErrorAttributes = "error_attributes";

    #endregion
    
    public const string ResponseBody = "response_body";
    public const string ResponseCookies = "response_cookies";
    public const string DataBag = "data_bag";
    public const string RequestHeaders = "request_headers";
    public const string ResponseHeaders = "response_headers";
    public const string Next = "next";
    public const string Error = "error";
    public const string Status = "status";
    public const string Session = "sessionsession";
}