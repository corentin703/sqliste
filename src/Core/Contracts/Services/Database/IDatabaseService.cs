using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Contracts.Services.Database;

public interface IDatabaseService
{
    public Task<List<IDictionary<string, object>>?> QueryAsync(
        string query, 
        Dictionary<string, object?> parameters, 
        CancellationToken cancellationToken = default
    ); 
    public Task<List<IDictionary<string, object>>?> QueryAsync(
        string query, 
        object? parameters = null, 
        CancellationToken cancellationToken = default
    ); 
    public Task<List<T>?> QueryAsync<T>(
        string query, 
        Dictionary<string, object?> parameters, 
        CancellationToken cancellationToken = default
    ); 
    public Task<List<T>?> QueryAsync<T>(
        string query, 
        object? parameters = null, 
        CancellationToken cancellationToken = default
    );

    public Task<List<T>?> ExecAsync<T>(
        string procedure,
        List<ProcedureArgumentModel> parameters,
        Dictionary<string, object?> parameterValues,
        CancellationToken cancellationToken = default
    );
}