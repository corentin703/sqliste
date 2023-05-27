using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Exceptions.Procedures;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Models.SqlAnnotations;
using Sqliste.Core.Models.SqlAnnotations.HttpMethods;
using Sqliste.Core.Models.SqlAnnotations.OpenApi;
using Sqliste.Core.Utils.SqlAnnotations;
using Sqliste.Database.Common.Contracts.Services;

namespace Sqliste.Infrastructure.Services;

internal class IntrospectionService : IIntrospectionService
{
    private const string ProcedureToRoutePattern = @"p_(?<resource>\w+)_(?<action>\w+)";

    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<IntrospectionService> _logger;
    private readonly IDatabaseGateway _databaseGateway;

    private const string IntrospectionCacheKey = "DatabaseIntrospection";

    public IntrospectionService(IMemoryCache memoryCache, ILogger<IntrospectionService> logger, IDatabaseGateway databaseGateway)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _databaseGateway = databaseGateway;
    }

    public async Task<DatabaseIntrospectionModel> IntrospectAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Reading database introspection");
        return await GetIntrospectionAsync(cancellationToken);
    }

    public void Clear()
    {
        _memoryCache.Remove(IntrospectionCacheKey);
    }

    private async Task<DatabaseIntrospectionModel> GetIntrospectionAsync(CancellationToken cancellationToken)
    {
        DatabaseIntrospectionModel? introspectionModel;

        if (!_memoryCache.TryGetValue(IntrospectionCacheKey, out introspectionModel) || introspectionModel == null)
        {
            introspectionModel = await RunIntrospectionAsync(cancellationToken);
            
            _logger.LogInformation("Caching database introspection");
            _memoryCache.Set(IntrospectionCacheKey, introspectionModel);
        }
        else
            _logger.LogDebug("Database introspection retrieved");

        return introspectionModel;
    }

    private async Task<DatabaseIntrospectionModel> RunIntrospectionAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting database introspection");

        List<ProcedureModel> procedures = await RunProceduresIntrospectionAsync(cancellationToken);

        List<ProcedureModel> endpoints = procedures
            .Where(procedure =>
                procedure.Annotations.All(annotation => annotation is not MiddlewareSqlAnnotation)
            )
            .ToList();

        Func<ProcedureModel, int> sortMiddleware = middleware =>
        {
            MiddlewareSqlAnnotation annotation = (MiddlewareSqlAnnotation) middleware
                .Annotations
                .First(annotation => annotation is MiddlewareSqlAnnotation);

            middleware.Route = annotation.PathStarts;
            return annotation.Order;
        };

        List<ProcedureModel> beforeMiddlewares = procedures
            .Where(procedure =>
                procedure.Annotations.Any(annotation => annotation is MiddlewareSqlAnnotation {After: false})
            )
            .OrderBy(sortMiddleware)
            .ToList();

        List<ProcedureModel> afterMiddlewares = procedures
            .Where(procedure =>
                procedure.Annotations.Any(annotation => annotation is MiddlewareSqlAnnotation {After: true})
            )
            .OrderBy(sortMiddleware)
            .ToList();

        _logger.LogInformation("Database introspection ended");
        
        return new DatabaseIntrospectionModel()
        {
            Endpoints = endpoints,
            BeforeMiddlewares = beforeMiddlewares,
            AfterMiddlewares = afterMiddlewares,
        };
    }

    private async Task<List<ProcedureModel>> RunProceduresIntrospectionAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting procedures introspection");

        List<ProcedureModel> procedures = await _databaseGateway.QueryProceduresAsync(cancellationToken);
        foreach (ProcedureModel procedure in procedures)
        {
            try
            {
                await RunProcedureIntrospectionAsync(procedure, cancellationToken);
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogWarning(exception: exception, "Error during procedure introspection");
            }
        }

        procedures = procedures.OrderByDescending(procedure => procedure.RoutePattern.Length).ToList();
        _memoryCache.Set(IntrospectionCacheKey, procedures);
        _logger.LogInformation("Database introspection ended");

        return procedures;
    }

    private async Task RunProcedureIntrospectionAsync(
        ProcedureModel procedure,
        CancellationToken cancellationToken = default
    )
    {
        procedure.Annotations = SqlAnnotationParser.ParseSqlString(procedure.Content);
        procedure.Arguments = await _databaseGateway.QueryProceduresParamsAsync(procedure.Name, cancellationToken);
        SetupRoutePattern(procedure);
        SetupQueryParams(procedure);
        SetupHttpMethod(procedure);
        SetupContentType(procedure);
    }

    private void SetupQueryParams(ProcedureModel procedure)
    {
        foreach (ProcedureArgumentModel argument in procedure.Arguments)
        {
            if (argument.IsSystemParam)
                continue;
            
            if (procedure.UriParams.Any(routeParam => routeParam.Name == argument.Name))
                continue;
            
            HttpUriParam queryParam = new()
            {
                Name = argument.Name,
                IsRequired = false,
                Location = ParameterLocation.Query,
            };
            
            procedure.UriParams.Add(queryParam);
        }

        int queryParamsCount = procedure.UriParams.Count(uriParam => uriParam.Location == ParameterLocation.Query);
        _logger.LogDebug("Found {ParamsCount} for {ProcedureName}", queryParamsCount, procedure.Name);
    }

    private void SetupRoutePattern(ProcedureModel procedure)
    {
        RouteSqlAnnotation? routeAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is RouteSqlAnnotation) as RouteSqlAnnotation;

        string routePattern;
        
        if (routeAnnotation == null)
            (routePattern, procedure.Operations) = GetDefaultRoutePattern(procedure);
        else
            routePattern = routeAnnotation.Path;

        procedure.Route = ConvertRoutePatternToHttpRoute(routePattern);

        _logger.LogDebug("Found {Pattern} for {ProcedureName}", routePattern, procedure.Name);

        List<HttpUriParam> routeParams = new();

        foreach (Match paramMatch in Regex.Matches(routePattern, @"{(?<name>\w+\??)}"))
        {
            if (!paramMatch.Success)
            {
                _logger.LogWarning("Can't read param for procedure {ProcedureName}", procedure.Name);
                continue;
            }

            string paramName = paramMatch.Groups["name"].Value;
            if (string.IsNullOrEmpty(paramName))
            {
                _logger.LogWarning("Reading empty param name for procedure {ProcedureName}", procedure.Name);
                continue;
            }

            HttpUriParam routeParam = new()
            {
                Name = paramName.Replace("?", ""),
                IsRequired = !paramName.Contains("?"),
            };
            
            ProcedureArgumentModel? procedureArgument = procedure.Arguments.FirstOrDefault(arg => arg.Name == routeParam.Name);
            if (procedureArgument == null)
            {
                _logger.LogWarning(
                    "Path param {ParamName} not existing in procedure {ProcedureName}'s arguments", 
                    paramName, 
                    procedure.Name
                );
            }
            else
                procedureArgument.Location = ParameterLocation.Path;

            routeParams.Add(routeParam);

            if (routeParam.IsRequired)
                routePattern = routePattern.Replace($"{{{paramName}}}", $@"(?<{routeParam.Name}>\w+)");
            else
            {
                routePattern = routePattern.Replace($"{{{paramName}}}", "").TrimEnd('/');
                routePattern = $@"{routePattern}/?(?<{routeParam.Name}>\w+)?";
            }
        }

        _logger.LogDebug("Found {ParamsCount} in {RoutePattern} for {ProcedureName}", routeParams.Count, routePattern, procedure.Name);

        procedure.RoutePattern = @$"^{routePattern}$";
        procedure.UriParams = routeParams;
    }

    private string ConvertRoutePatternToHttpRoute(string routePattern)
    {
        return routePattern.Replace("?", "");
    }
    
    private void SetupHttpMethod(ProcedureModel procedure)
    {
        List<HttpMethodBaseSqlAnnotation> methodAnnotations = procedure.Annotations
            .Where(annotation => annotation is HttpMethodBaseSqlAnnotation)
            .Cast<HttpMethodBaseSqlAnnotation>()
            .ToList();

        if (methodAnnotations.Count == 0)
            return;

        List<HttpOperationModel> httpOperations = new();

        AddHttpMethod<HttpGetSqlAnnotation>(httpOperations, methodAnnotations);
        AddHttpMethod<HttpPostSqlAnnotation>(httpOperations, methodAnnotations);
        AddHttpMethod<HttpPutSqlAnnotation>(httpOperations, methodAnnotations);
        AddHttpMethod<HttpPatchSqlAnnotation>(httpOperations, methodAnnotations);
        AddHttpMethod<HttpDeleteSqlAnnotation>(httpOperations, methodAnnotations);

        procedure.Operations = httpOperations.ToArray();
    }

    private void AddHttpMethod<TAnnotation>(List<HttpOperationModel> httpOperations, List<HttpMethodBaseSqlAnnotation> methodAnnotations)
        where TAnnotation : HttpMethodBaseSqlAnnotation
    {
        HttpMethodBaseSqlAnnotation? method = methodAnnotations
            .FirstOrDefault(annotation => annotation is TAnnotation);
        
        if (method != null)
        {
            httpOperations.Add(new HttpOperationModel()
            {
                Id = method.Id,
                Method = method.Method,
            });
        }
    }

    private void SetupContentType(ProcedureModel procedure)
    {
        ProducesSqlAnnotation? producesAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is ProducesSqlAnnotation) as ProducesSqlAnnotation;

        if (producesAnnotation == null)
            return;

        procedure.ContentType = producesAnnotation.Mime;
    }

    private (string, HttpOperationModel[]) GetDefaultRoutePattern(ProcedureModel procedure)
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
        {
            return ($"/{resource}", new []
            {
                new HttpOperationModel()
                {
                    Method = httpMethod,
                }
            });
        }

        return ($"/{resource}/{action}", procedure.Operations);
    }
}