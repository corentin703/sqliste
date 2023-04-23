using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Contracts.Services;

public interface IDatabaseIntrospectionService
{
    public Task<List<ProcedureModel>> IntrospectAsync(CancellationToken cancellationToken = default);
    public void Clear();
}