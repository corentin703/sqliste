using Sqliste.Core.Models;

namespace Sqliste.Core.Contracts.Services;

public interface IDatabaseIntrospectionService
{
    public Task<List<ProcedureModel>> IntrospectAsync(CancellationToken cancellationToken = default);
}