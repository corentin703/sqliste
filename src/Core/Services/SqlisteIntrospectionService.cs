﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Exceptions.Procedures;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.SqlAnnotations;
using Sqliste.Core.SqlAnnotations.HttpMethods;
using Sqliste.Core.SqlAnnotations.OpenApi;
using Sqliste.Core.Utils.SqlAnnotations;
using System.Text.RegularExpressions;
using Sqliste.Core.Contracts.Services.Database;

namespace Sqliste.Core.Services;

public class SqlisteIntrospectionService : ISqlisteIntrospectionService
{
    private const string ProcedureToRoutePattern = @"p_(?<resource>\w+)_(?<action>\w+)";

    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<SqlisteIntrospectionService> _logger;
    private readonly IDatabaseGatewayService _databaseGatewayService;

    private const string IntrospectionCacheKey = "DatabaseIntrospection";

    public SqlisteIntrospectionService(IMemoryCache memoryCache, ILogger<SqlisteIntrospectionService> logger, IDatabaseGatewayService databaseGatewayService)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _databaseGatewayService = databaseGatewayService;
    }

    public virtual async Task<DatabaseIntrospectionModel> IntrospectAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Reading database introspection");
        List<ProcedureModel>? procedures;

        if (!_memoryCache.TryGetValue(IntrospectionCacheKey, out procedures) || procedures == null)
        {
            _logger.LogInformation("Starting database introspection");
            procedures = await _databaseGatewayService.QueryProceduresAsync(cancellationToken);
            foreach (ProcedureModel procedure in procedures)
            {
                try
                {
                    await IntrospectProcedureAsync(procedure, cancellationToken);
                }
                catch (InvalidOperationException exception)
                {
                    _logger.LogWarning("Exception during introspection: {Exception}", exception.Message);
                }
            }

            procedures = procedures.OrderByDescending(procedure => procedure.RoutePattern.Length).ToList();
            _memoryCache.Set(IntrospectionCacheKey, procedures);
            _logger.LogInformation("Database introspection ended");
        }
        
        _logger.LogDebug("Database introspection retrieved");

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
            //.Select(formatMiddleware)
            .OrderBy(sortMiddleware)
            .ToList();

        List<ProcedureModel> afterMiddlewares = procedures
            .Where(procedure =>
                procedure.Annotations.Any(annotation => annotation is MiddlewareSqlAnnotation {After: true})
            )
            //.Select(formatMiddleware)
            .OrderBy(sortMiddleware)
            .ToList();

        return new DatabaseIntrospectionModel()
        {
            Endpoints = endpoints,
            BeforeMiddlewares = beforeMiddlewares,
            AfterMiddlewares = afterMiddlewares,
        };
    }

    public void Clear()
    {
        _memoryCache.Remove(IntrospectionCacheKey);
    }

    protected virtual async Task IntrospectProcedureAsync(
        ProcedureModel procedure,
        CancellationToken cancellationToken = default
    )
    {
        procedure.Annotations = SqlAnnotationParser.ParseSqlString(procedure.Content);
        procedure.Arguments = await _databaseGatewayService.QueryProceduresParamsAsync(procedure.Name, cancellationToken);
        SetupRoutePattern(procedure);
        SetupHttpMethod(procedure);
        SetupContentType(procedure);
    }

    private void SetupRoutePattern(ProcedureModel procedure)
    {
        RouteSqlAnnotation? routeAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is RouteSqlAnnotation) as RouteSqlAnnotation;

        if (routeAnnotation == null)
            (procedure.Route, procedure.Operations) = GetDefaultRoutePattern(procedure);
        else
            procedure.Route = routeAnnotation.Path;

        string routePattern = procedure.Route;

        _logger.LogDebug("Found {Pattern} for {ProcedureName}", routePattern, procedure.Name);

        List<string> paramsNames = new();
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

            ProcedureArgumentModel? procedureArgument = procedure.Arguments.FirstOrDefault(arg => arg.Name == paramName);
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

            paramsNames.Add(paramName);
            routePattern = routePattern.Replace($"{{{paramName}}}", $@"(?<{paramName}>\w+)");
        }

        _logger.LogDebug("Found {ParamsCount} in {RoutePattern} for {ProcedureName}", paramsNames.Count, routePattern, procedure.Name);

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

        List<HttpOperationModel> httpOperations = new();

        HttpMethodBaseSqlAnnotation? getMethod =
            methodAnnotations.FirstOrDefault(annotation => annotation is HttpGetSqlAnnotation);
        if (getMethod != null)
        {
            httpOperations.Add(new HttpOperationModel()
            {
                Id = getMethod.Id,
                Method = HttpMethod.Get,
            });
        }

        HttpMethodBaseSqlAnnotation? postMethod =
            methodAnnotations.FirstOrDefault(annotation => annotation is HttpPostSqlAnnotation);
        if (postMethod != null)
        {
            httpOperations.Add(new HttpOperationModel()
            {
                Id = postMethod.Id,
                Method = HttpMethod.Post,
            });
        }

        HttpMethodBaseSqlAnnotation? putMethod =
            methodAnnotations.FirstOrDefault(annotation => annotation is HttpPutSqlAnnotation);
        if (putMethod != null)
        {
            httpOperations.Add(new HttpOperationModel()
            {
                Id = putMethod.Id,
                Method = HttpMethod.Put,
            });
        }

        HttpMethodBaseSqlAnnotation? patchMethod =
            methodAnnotations.FirstOrDefault(annotation => annotation is HttpPatchSqlAnnotation);
        if (patchMethod != null)
        {
            httpOperations.Add(new HttpOperationModel()
            {
                Id = patchMethod.Id,
                Method = HttpMethod.Patch,
            });
        }

        HttpMethodBaseSqlAnnotation? deleteMethod =
            methodAnnotations.FirstOrDefault(annotation => annotation is HttpDeleteSqlAnnotation);
        if (deleteMethod != null)
        {
            httpOperations.Add(new HttpOperationModel()
            {
                Id = deleteMethod.Id,
                Method = HttpMethod.Delete,
            });
        }

        procedure.Operations = httpOperations.ToArray();
    }

    private void SetupContentType(ProcedureModel procedure)
    {
        ProducesSqlAnnotation? producesAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is ProducesSqlAnnotation) as ProducesSqlAnnotation;

        if (producesAnnotation == null)
            return;

        procedure.ContentType = producesAnnotation.Mime;
    }

    protected virtual (string, HttpOperationModel[]) GetDefaultRoutePattern(ProcedureModel procedure)
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