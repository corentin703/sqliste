using Microsoft.Extensions.Logging;
using Sqliste.Core.Constants;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Utils.Uri;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Sqliste.Core.Services;

public abstract class RequestHandlerService : IRequestHandlerService
{
    protected readonly IDatabaseIntrospectionService DatabaseIntrospectionService;
    protected readonly IDatabaseService DatabaseService;
    private readonly ILogger<RequestHandlerService> _logger;

    protected RequestHandlerService(IDatabaseIntrospectionService databaseIntrospectionService, IDatabaseService databaseService, ILogger<RequestHandlerService> logger)
    {
        DatabaseIntrospectionService = databaseIntrospectionService;
        DatabaseService = databaseService;
        _logger = logger;
    }

    public async Task<HttpRequestModel> HandleRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default)
    {
        DatabaseIntrospectionModel introspection = await DatabaseIntrospectionService.IntrospectAsync(cancellationToken);

        ProcedureModel? procedure = introspection.Endpoints
            .FirstOrDefault(route => IsMatchingRoute(request, route) && IsMatchingVerb(request, route));

        if (procedure == null)
        {
            _logger.LogInformation("Matching procedure not found for path {path}", request.Path);
            return new HttpRequestModel()
            {
                Status = HttpStatusCode.NotFound,
            };
        }

        _logger.LogDebug("Procedure found for path {path} : {procedureName}", request.Path, procedure.Name);

        request.PathParams = ParseUriParams(request.Path, procedure);
        
        await ExecMiddlewaresAsync(request, introspection.BeforeMiddlewares, cancellationToken);
        if (!request.Next)
            return request;

        request = await ExecRequestAsync(request, procedure, cancellationToken) ?? request;
        await ExecMiddlewaresAsync(request, introspection.AfterMiddlewares, cancellationToken);

        request.ContentType = procedure.ContentType;
        return request;
    }

    private async Task ExecMiddlewaresAsync(
        HttpRequestModel request, 
        List<ProcedureModel> middlewares,
        CancellationToken cancellationToken = default
    )
    {
        List<ProcedureModel> middlewaresToRun = middlewares
            .Where(middleware => request.Path.StartsWith(middleware.Route))
            .ToList();

        foreach (ProcedureModel middleware in middlewaresToRun)
        {
            Dictionary<string, object?> sqlParams = GetParams(request, middleware);
            HttpRequestModel? middlewareResponse = await ExecProcedureAsync(middleware, sqlParams, cancellationToken);

            if (middlewareResponse == null)
                continue;

            if (!middlewareResponse.Next)
                break;

            if (middlewareResponse.Body != null)
                request.Body = middlewareResponse.Body;

            if (middlewareResponse.Cookies != null)
                request.Cookies = middlewareResponse.Cookies;

            if (middlewareResponse.DataBag != null)
                request.DataBag = middlewareResponse.DataBag;

            if (middlewareResponse.Headers != null)
                request.Headers = middlewareResponse.Headers;
        }
    }

    private async Task<HttpRequestModel?> ExecRequestAsync(
        HttpRequestModel request, 
        ProcedureModel procedure,
        CancellationToken cancellationToken = default
    )
    {
        Dictionary<string, object?> sqlParams = GetParams(request, procedure);
        return await ExecProcedureAsync(procedure, sqlParams, cancellationToken);
    }

    protected abstract Task<HttpRequestModel?> ExecProcedureAsync(
        ProcedureModel procedure, 
        Dictionary<string, object?> sqlParams, 
        CancellationToken cancellationToken
    );

    private Dictionary<string, object?> GetParams(HttpRequestModel request, ProcedureModel procedure)
    {
        Dictionary<string, object?> sqlParams = new();

        foreach (KeyValuePair<string, string> uriParam in request.PathParams)
        {
            sqlParams.TryAdd(uriParam.Key, uriParam.Value);
        }

        if (request.QueryString != null)
        {
            Dictionary<string, string> queryParams = UriQueryParamsParser.ParseQueryParams(request.QueryString);
            foreach (KeyValuePair<string, string> queryParam in queryParams)
            {
                sqlParams.TryAdd(queryParam.Key, queryParam.Value);
            }
        }

        sqlParams.TryAdd(SystemQueryParametersConstants.Body, request.Body);
        sqlParams.TryAdd(SystemQueryParametersConstants.Body, request.Body);
        sqlParams.TryAdd(SystemQueryParametersConstants.Cookies, request.Cookies);
        sqlParams.TryAdd(SystemQueryParametersConstants.DataBag, request.DataBag);
        sqlParams.TryAdd(SystemQueryParametersConstants.Headers, request.Headers);
        sqlParams.TryAdd(SystemQueryParametersConstants.PathParams, JsonSerializer.Serialize(request.PathParams));

        _logger.LogDebug("Added {paramCount} for {procedureName}", sqlParams.Count, procedure.Name);
        return sqlParams;
    }

    private bool IsMatchingRoute(HttpRequestModel request, ProcedureModel procedure)
    {
        return Regex.IsMatch(request.Path, procedure.RoutePattern);
    }

    private bool IsMatchingVerb(HttpRequestModel request, ProcedureModel procedure)
    {
        return procedure.HttpMethods.Any(method => method == request.Method);
    }

    private Dictionary<string, string> ParseUriParams(string uri, ProcedureModel procedure)
    {
        Dictionary<string, string> pathParams = new();

        Match paramsMatch = Regex.Match(uri, procedure.RoutePattern);
        if (!paramsMatch.Success)
        {
            _logger.LogDebug("Path params's pattern didn't match for {procedureName} (path: {path})", procedure.Name, uri);
            return pathParams;
        }

        procedure.RouteParamNames.ForEach(paramName =>
        {
            string paramValue = paramsMatch.Groups[paramName].Value;
            if (string.IsNullOrEmpty(paramValue))
                return;

            pathParams.Add(paramName, paramValue);
        });

        _logger.LogDebug("Found {paramCount} path params for {procedureName} (path: {path})", pathParams.Count, procedure.Name, uri);
        return pathParams;
    }
}