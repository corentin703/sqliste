using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Contracts.Services;

public interface IParametersResolver
{
    public Task<Dictionary<string, object?>> GetParamsAsync(
        PipelineBag pipeline, 
        ProcedureModel procedure,
        CancellationToken cancellationToken
    );
}