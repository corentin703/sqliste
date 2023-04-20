using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models;
using Sqliste.Core.Utils.SqlAnnotations;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerIntrospectionService : IDatabaseIntrospectionService
{
    private readonly IDatabaseService _databaseService;

    public SqlServerIntrospectionService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<ProcedureModel>> IntrospectAsync(CancellationToken cancellationToken = default)
    {
        List<ProcedureModel> procedures = await QueryProceduresAsync(cancellationToken);
        foreach (ProcedureModel procedure in procedures)
        {
            procedure.Annotations = SqlAnnotationParser.ParseSqlString(procedure.Content);
            await QueryProceduresParamsAsync(procedure, cancellationToken);
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
}