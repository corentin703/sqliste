using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Exceptions.Procedures;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.SqlAnnotations;
using Sqliste.Core.Utils.SqlAnnotations;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerIntrospectionService : IDatabaseIntrospectionService
{
    private readonly IDatabaseService _databaseService;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<SqlServerIntrospectionService> _logger;

    private const string IntrospectionCacheKey = "DatabaseIntrospection";

    public SqlServerIntrospectionService(IDatabaseService databaseService, IMemoryCache memoryCache, ILogger<SqlServerIntrospectionService> logger)
    {
        _databaseService = databaseService;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<List<ProcedureModel>> IntrospectAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Reading database introspection");
        List<ProcedureModel>? procedures;

        if (!_memoryCache.TryGetValue(IntrospectionCacheKey, out procedures) || procedures == null)
        {
            _logger.LogInformation("Starting database introspection");
            procedures = await QueryProceduresAsync(cancellationToken);
            foreach (ProcedureModel procedure in procedures)
            {
                try
                {
                    procedure.Annotations = SqlAnnotationParser.ParseSqlString(procedure.Content);
                    SetupRoutePattern(procedure);
                    procedure.Arguments = await QueryProceduresParamsAsync(procedure.Name, cancellationToken);
                }
                catch (InvalidOperationException exception)
                {
                    _logger.LogWarning(exception.Message);
                }
            }

            procedures = procedures.OrderByDescending(procedure => procedure.RoutePattern.Length).ToList();
            _memoryCache.Set(IntrospectionCacheKey, procedures);
            _logger.LogInformation("Database introspection ended");
        }
        
        _logger.LogDebug("Database introspection retrieved");
        return procedures;
    }

    private async Task<List<ProcedureModel>> QueryProceduresAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Querying procedures");
        (string query, object args) = IntrospectionSqlQueries.GetProceduresQuery();
        List<ProcedureModel>? procedures = await _databaseService.QueryAsync<ProcedureModel>(query, args, cancellationToken);

        if (procedures == null || procedures.Count == 0)
        {
            _logger.LogWarning("No procedures available");
            return new List<ProcedureModel>();
        }

        _logger.LogInformation("Queried {number}", procedures.Count);
        return procedures;
    }

    private async Task<List<ArgumentModel>> QueryProceduresParamsAsync(string procedureName, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Querying procedure params for {procedureName}", procedureName);
        (string query, object args) = IntrospectionSqlQueries.GetProceduresArgumentsQuery(procedureName);
        List<ArgumentModel>? procedureArgs = await _databaseService.QueryAsync<ArgumentModel>(query, args, cancellationToken);

        procedureArgs ??= new List<ArgumentModel>();

        _logger.LogInformation("Queried {number} params for {procedureName}", procedureArgs.Count, procedureName);

        return procedureArgs;
    }

    private void SetupRoutePattern(ProcedureModel procedure)
    {
        RouteSqlAnnotation? routeAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is RouteSqlAnnotation) as RouteSqlAnnotation;

        string routePattern = routeAnnotation == null 
            ? GetDefaultRoutePattern(procedure) 
            : routeAnnotation.Path
        ;

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

    private const string ProcedureToRoutePattern = @"p_(?<resource>\w+)_(?<action>\w+)";
    private string GetDefaultRoutePattern(ProcedureModel procedure)
    {
        Match procedureToRouteMatch = Regex.Match(procedure.Name, ProcedureToRoutePattern);

        if (!procedureToRouteMatch.Success)
        {
            throw new RouteDeductionException(procedure.Name);
        }

        string resource = procedureToRouteMatch.Groups["resource"].Value;
        string action = procedureToRouteMatch.Groups["action"].Value;

        if (string.IsNullOrWhiteSpace(resource) || string.IsNullOrWhiteSpace(action)) 
            throw new RouteDeductionException(procedure.Name);

        return $"/{resource}/{action}";
    }
}