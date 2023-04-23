using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Exceptions.Procedures;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.SqlAnnotations;
using Sqliste.Core.SqlAnnotations.HttpMethods;
using Sqliste.Core.Utils.SqlAnnotations;

namespace Sqliste.Core.Services;

public abstract class DatabaseIntrospectionService : IDatabaseIntrospectionService
{
    private const string ProcedureToRoutePattern = @"p_(?<resource>\w+)_(?<action>\w+)";

    protected readonly IDatabaseService DatabaseService;
    protected readonly IMemoryCache MemoryCache;
    private readonly ILogger<DatabaseIntrospectionService> _logger;

    private const string IntrospectionCacheKey = "DatabaseIntrospection";

    protected DatabaseIntrospectionService(IDatabaseService databaseService, IMemoryCache memoryCache, ILogger<DatabaseIntrospectionService> logger)
    {
        DatabaseService = databaseService;
        MemoryCache = memoryCache;
        _logger = logger;
    }

    public virtual async Task<List<ProcedureModel>> IntrospectAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Reading database introspection");
        List<ProcedureModel>? procedures;

        if (!MemoryCache.TryGetValue(IntrospectionCacheKey, out procedures) || procedures == null)
        {
            _logger.LogInformation("Starting database introspection");
            procedures = await QueryProceduresAsync(cancellationToken);
            foreach (ProcedureModel procedure in procedures)
            {
                try
                {
                    await IntrospectProcedureAsync(procedure, cancellationToken);
                }
                catch (InvalidOperationException exception)
                {
                    _logger.LogWarning(exception.Message);
                }
            }

            procedures = procedures.OrderByDescending(procedure => procedure.RoutePattern.Length).ToList();
            MemoryCache.Set(IntrospectionCacheKey, procedures);
            _logger.LogInformation("Database introspection ended");
        }
        
        _logger.LogDebug("Database introspection retrieved");
        return procedures;
    }

    public void Clear()
    {
        MemoryCache.Remove(IntrospectionCacheKey);
    }

    protected abstract Task<List<ProcedureModel>> QueryProceduresAsync(CancellationToken cancellationToken = default);

    protected abstract Task<List<ArgumentModel>> QueryProceduresParamsAsync(string procedureName, CancellationToken cancellationToken = default);

    protected virtual async Task IntrospectProcedureAsync(
        ProcedureModel procedure,
        CancellationToken cancellationToken = default
    )
    {
        procedure.Annotations = SqlAnnotationParser.ParseSqlString(procedure.Content);
        SetupRoutePattern(procedure);
        SetupHttpMethod(procedure);
        SetupContentType(procedure);
        procedure.Arguments = await QueryProceduresParamsAsync(procedure.Name, cancellationToken);
    }

    private void SetupRoutePattern(ProcedureModel procedure)
    {
        RouteSqlAnnotation? routeAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is RouteSqlAnnotation) as RouteSqlAnnotation;

        string routePattern;

        if (routeAnnotation == null)
            (routePattern, procedure.HttpMethods) = GetDefaultRoutePattern(procedure);
        else
            routePattern = routeAnnotation.Path;

        _logger.LogDebug("Found {pattern} for {procedureName}", routePattern, procedure.Name);

        List<string> paramsNames = new();
        foreach (Match paramMatch in Regex.Matches(routePattern, @"{(?<name>\w+\??)}"))
        {
            if (!paramMatch.Success)
            {
                _logger.LogWarning("Can't read param for procedure {procedureName}", procedure.Name);
                continue;
            }

            string paramName = paramMatch.Groups["name"].Value;
            if (string.IsNullOrEmpty(paramName))
            {
                _logger.LogWarning("Reading empty param name for procedure {procedureName}", procedure.Name);
                continue;
            }
            
            paramsNames.Add(paramName);
            routePattern = routePattern.Replace($"{{{paramName}}}", $@"(?<{paramName}>\w+)");
        }

        _logger.LogDebug("Found {paramsCount} in {routePattern} for {procedureName}", paramsNames.Count, routePattern, procedure.Name);

        procedure.RoutePattern = @$"^{routePattern}$";
        procedure.RouteParamNames = paramsNames;
    }
    private void SetupHttpMethod(ProcedureModel procedure)
    {
        List<HttpMethodBaseSqlAnnotation> methodAnnotations = procedure.Annotations
            .Where(annotation => annotation is HttpMethodBaseSqlAnnotation)
            .Cast<HttpMethodBaseSqlAnnotation>()
            .ToList();

        if (methodAnnotations.Count == 0)
            return;

        List<HttpMethod> httpMethods = new();

        if (methodAnnotations.Any(annotation => annotation is HttpGetSqlAnnotation))
            httpMethods.Add(HttpMethod.Get);

        if (methodAnnotations.Any(annotation => annotation is HttpPostSqlAnnotation))
            httpMethods.Add(HttpMethod.Post);

        if (methodAnnotations.Any(annotation => annotation is HttpPutSqlAnnotation))
            httpMethods.Add(HttpMethod.Put);

        if (methodAnnotations.Any(annotation => annotation is HttpPatchSqlAnnotation))
            httpMethods.Add(HttpMethod.Delete);

        procedure.HttpMethods = httpMethods.ToArray();
    }

    private void SetupContentType(ProcedureModel procedure)
    {
        ProducesSqlAnnotation? producesAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is ProducesSqlAnnotation) as ProducesSqlAnnotation;

        if (producesAnnotation == null)
            return;

        procedure.ContentType = producesAnnotation.Mime;
    }

    protected virtual (string, HttpMethod[]) GetDefaultRoutePattern(ProcedureModel procedure)
    {
        HttpMethod? httpMethod = null;
        Match procedureToRouteMatch = Regex.Match(procedure.Name, ProcedureToRoutePattern);

        if (!procedureToRouteMatch.Success)
        {
            throw new RouteDeductionException(procedure.Name);
        }

        string resource = procedureToRouteMatch.Groups["resource"].Value;
        string action = procedureToRouteMatch.Groups["action"].Value;

        if (string.IsNullOrWhiteSpace(resource) || string.IsNullOrWhiteSpace(action)) 
            throw new RouteDeductionException(procedure.Name);

        switch (action.ToLower())
        {
            case "get":
                httpMethod = HttpMethod.Get;
                break;
            case "post":
                httpMethod = HttpMethod.Post;
                break;
            case "put":
                httpMethod = HttpMethod.Put;
                break;
            case "patch":
                httpMethod = HttpMethod.Patch;
                break;
            case "delete":
                httpMethod = HttpMethod.Delete;
                break;
        }

        if (httpMethod != null)
            return ($"/{resource}", new [] {httpMethod});

        return ($"/{resource}/{action}", procedure.HttpMethods);
    }
}