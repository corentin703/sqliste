using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Contracts.Services.Database;

public interface IDatabaseGatewayService
{
    public Task<PipelineResponseBag?> ExecProcedureAsync(
        PipelineRequestBag request,
        ProcedureModel procedure,
        Dictionary<string, object?> sqlParams,
        CancellationToken cancellationToken
    );

    public Task<List<ProcedureModel>> QueryProceduresAsync(CancellationToken cancellationToken = default);
    public Task<List<ProcedureArgumentModel>> QueryProceduresParamsAsync(string procedureName, CancellationToken cancellationToken = default);
}