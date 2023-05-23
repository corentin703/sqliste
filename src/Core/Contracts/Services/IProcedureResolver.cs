using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Contracts.Services;

public interface IProcedureResolver
{
    public Task<ProcedureModel?> ResolveProcedureAsync(string path, HttpMethod method, CancellationToken cancellationToken = default);
}