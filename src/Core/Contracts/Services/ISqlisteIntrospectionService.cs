using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Contracts.Services;

public interface ISqlisteIntrospectionService
{
    public Task<DatabaseIntrospectionModel> IntrospectAsync(CancellationToken cancellationToken = default);
    public void Clear();
}