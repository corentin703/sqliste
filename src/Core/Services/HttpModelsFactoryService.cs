using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Sqliste.Core.Exceptions.Services.HttpModelFactoryService;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Utils.Uri;

namespace Sqliste.Core.Services;

public class HttpModelsFactoryService : IHttpModelsFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpModelsFactoryService> _logger;
    private readonly IProcedureResolverService _procedureResolver;
    
    public HttpModelsFactoryService(IHttpContextAccessor httpContextAccessor, ILogger<HttpModelsFactoryService> logger, IProcedureResolverService procedureResolver)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _procedureResolver = procedureResolver;
    }

    public async Task<PipelineBag> BuildRequestModelAsync(CancellationToken cancellationToken = default)
    {
        HttpRequest request = _httpContextAccessor.HttpContext.Request;

        HttpMethod httpMethod = GetHttpMethodFromString(request.Method);
        string? bodyContent = await ParseRequestBodyAsync(request, cancellationToken);

        Dictionary<string, string> cookies =
            request.Cookies.ToDictionary(cookie => cookie.Key, cookie => cookie.Value);

        Dictionary<string, string> headers =
            request.Headers.ToDictionary(header => header.Key, header => header.Value.ToString());
        
        string? queryString = request.QueryString.HasValue
            ? request.QueryString.Value
            : null
        ;

        Dictionary<string, string> queryParams = queryString != null
            ? UriQueryParamsParser.ParseQueryParams(queryString)
            : new()
        ;

        ProcedureModel? procedure = await _procedureResolver.ResolveProcedureAsync(request.Path, httpMethod, cancellationToken);
        
        Dictionary<string, string> pathParams = procedure != null 
            ? ParseUriParams(request.Path, procedure)
            : new();
        
        PipelineRequestBag requestModel = new()
        {
            Path = request.Path,
            QueryString = queryString,
            QueryParams = queryParams,
            PathParams = pathParams,
            Method = httpMethod,
            Body = bodyContent,
            Cookies = JsonSerializer.Serialize(cookies),
            Headers = JsonSerializer.Serialize(headers),
        };

        PipelineBag pipelineModel = new PipelineBag()
        {
            Request = requestModel,
            Procedure = procedure,
        };

        return pipelineModel;
    }

    private HttpMethod GetHttpMethodFromString(string method)
    {
        return method switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PATCH" => HttpMethod.Patch,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            _ => HttpMethod.Get
        };
    }

    private async Task<string?> ParseRequestBodyAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete)
            return null;

        if (request.ContentLength == 0)
            return null;

        try
        {
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
            int readResult = await request.Body.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            return Encoding.UTF8.GetString(buffer);
        }
        catch(Exception exception) 
        {
            _logger.LogError(exception: exception, "Unable to parse body");
            throw new RequestBodyParsingException();
        }
    }
    
    private Dictionary<string, string> ParseUriParams(string uri, ProcedureModel procedure)
    {
        Dictionary<string, string> pathParams = new();

        Match paramsMatch = Regex.Match(uri, procedure.RoutePattern);
        if (!paramsMatch.Success)
        {
            _logger.LogDebug("Path params's pattern didn't match for {ProcedureName} (path: {Path})", procedure.Name, uri);
            return pathParams;
        }

        procedure.UriParams.ForEach(routeParam =>
        {
            string paramValue = paramsMatch.Groups[routeParam.Name].Value;
            if (string.IsNullOrEmpty(paramValue))
                return;

            pathParams.Add(routeParam.Name, paramValue);
        });

        _logger.LogDebug("Found {ParamCount} path params for {ProcedureName} (path: {Path})", pathParams.Count, procedure.Name, uri);
        return pathParams;
    }
}