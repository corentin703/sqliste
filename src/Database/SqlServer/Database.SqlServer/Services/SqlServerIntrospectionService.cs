using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.SqlAnnotations;
using Sqliste.Core.Utils.SqlAnnotations;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerIntrospectionService : IDatabaseIntrospectionService
{
    private readonly IDatabaseService _databaseService;
    private readonly IMemoryCache _memoryCache;

    private const string IntrospectionCacheKey = "DatabaseIntrospection";

    public SqlServerIntrospectionService(IDatabaseService databaseService, IMemoryCache memoryCache)
    {
        _databaseService = databaseService;
        _memoryCache = memoryCache;
    }

    public async Task<List<ProcedureModel>> IntrospectAsync(CancellationToken cancellationToken = default)
    {
        List<ProcedureModel>? procedures;

        if (!_memoryCache.TryGetValue(IntrospectionCacheKey, out procedures) || procedures == null)
        {
            procedures = await QueryProceduresAsync(cancellationToken);
            foreach (ProcedureModel procedure in procedures)
            {
                procedure.Annotations = SqlAnnotationParser.ParseSqlString(procedure.Content);
                await QueryProceduresParamsAsync(procedure, cancellationToken);
                SetRoutePattern(procedure);
            }

            _memoryCache.Set(IntrospectionCacheKey, procedures);
        }
       
        return procedures;
    }

    private async Task<List<ProcedureModel>> QueryProceduresAsync(CancellationToken cancellationToken = default)
    {
        (string query, object args) = IntrospectionSqlQueries.GetProceduresQuery();
        List<ProcedureModel> procedures = await _databaseService.QueryAsync<ProcedureModel>(query, args, cancellationToken);

        return procedures;
    }

    private async Task<ProcedureModel> QueryProceduresParamsAsync(ProcedureModel procedure, CancellationToken cancellationToken = default)
    {
        (string query, object args) = IntrospectionSqlQueries.GetProceduresArgumentsQuery(procedure.Name);
        List<ArgumentModel> procedureArgs = await _databaseService.QueryAsync<ArgumentModel>(query, args, cancellationToken);

        procedure.Arguments = procedureArgs;

        return procedure;
    }

    private ProcedureModel SetRoutePattern(ProcedureModel procedure)
    {
        RouteSqlAnnotation? routeAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is RouteSqlAnnotation) as RouteSqlAnnotation;

        string route = routeAnnotation == null // TODO : Route must be optional (default to procedure naming based route)
            ? procedure.Name
            : routeAnnotation.Path
        ;

        procedure.RoutePattern = Regex.Replace(route, @"{(?<name>\w+\??)}", @"\w+");

        return procedure;
    }
}