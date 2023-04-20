namespace Sqliste.Core.Contracts.Services;

public interface IDatabaseService
{
    public Task<List<IDictionary<string, object>>> QueryAsync(string query, object parameters, CancellationToken cancellationToken = default); 
    public Task<List<T>> QueryAsync<T>(string query, object parameters, CancellationToken cancellationToken = default); 
}