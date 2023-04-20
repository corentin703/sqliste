using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Sqliste.Core.Contracts.Services;
using Sqliste.Database.SqlServer.Configuration;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerDatabaseService : IDatabaseService, IDisposable
{
    private readonly SqlConnection _sqlConnection;

    public SqlServerDatabaseService(IOptionsSnapshot<SqlServerConfiguration> configuration)
    {
        _sqlConnection = new SqlConnection(configuration.Value.ConnectionString);
    }

    public async Task<List<IDictionary<string, object>>> QueryAsync(string query, object parameters, CancellationToken cancellationToken = default)
    {
        IEnumerable<dynamic>? result = await _sqlConnection.QueryAsync(query, parameters);
        return result.Cast<IDictionary<string, object>>().ToList();
    }

    public async Task<List<T>> QueryAsync<T>(string query, object parameters, CancellationToken cancellationToken = default)
    {
        return (await _sqlConnection.QueryAsync<T>(query, parameters)).ToList();
    }

    public void Dispose()
    {
        _sqlConnection.Dispose();
    }
}