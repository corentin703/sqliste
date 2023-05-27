namespace Sqliste.Core.Extensions;

public static class HttpMethodExtensions
{
    public static HttpMethod Parse(string method)
    {
        return method.ToLower() switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
            "put" => HttpMethod.Put,
            "patch" => HttpMethod.Patch,
            "delete" => HttpMethod.Delete,
            "options" => HttpMethod.Options,
            "trace" => HttpMethod.Trace,
            "head" => HttpMethod.Head,
            "connect" => HttpMethod.Connect,
            _ => throw new InvalidOperationException("Can't convert string to HttpMethod"),
        };
    }
    
    public static void TryParse(string method, out HttpMethod? httpMethod)
    {
        try
        {
            httpMethod = Parse(method);
        }
        catch (InvalidOperationException)
        {
            httpMethod = null;
        }
    }
}