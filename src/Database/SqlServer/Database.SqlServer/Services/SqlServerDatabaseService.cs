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

    public async Task<List<IDictionary<string, object>>?> QueryAsync(string query, Dictionary<string, object?> parameters, CancellationToken cancellationToken = default)
    {
        (string formattedQuery, DynamicParameters queryParams) = EscapeParams(query, parameters);

        IEnumerable<dynamic>? result = await _sqlConnection.QueryAsync(formattedQuery, queryParams);
        return result?.Cast<IDictionary<string, object>>().ToList();
    }

    public async Task<List<T>?> QueryAsync<T>(string query, Dictionary<string, object?> parameters, CancellationToken cancellationToken = default)
    {
        (string formattedQuery, DynamicParameters queryParams) = EscapeParams(query, parameters);

        return (await _sqlConnection.QueryAsync<T>(formattedQuery, queryParams))?.ToList();
    }

    public async Task<List<IDictionary<string, object>>?> QueryAsync(string query, object parameters, CancellationToken cancellationToken = default)
    {
        IEnumerable<dynamic>? result = await _sqlConnection.QueryAsync(query, parameters);
        return result?.Cast<IDictionary<string, object>>().ToList();
    }

    public async Task<List<T>?> QueryAsync<T>(string query, object parameters, CancellationToken cancellationToken = default)
    {
        return (await _sqlConnection.QueryAsync<T>(query, parameters))?.ToList();
    }

    private (string, DynamicParameters) EscapeParams(string baseQuery, Dictionary<string, object?> parameters)
    {
        string formattedQuery = baseQuery;
        DynamicParameters queryParams = new DynamicParameters();

        foreach (KeyValuePair<string, object?> parameter in parameters)
        {
            string escapingAlias = $"{parameter.Key}_ESC";
            formattedQuery = $"{formattedQuery} @{parameter.Key} = @{escapingAlias},";
            queryParams.Add(escapingAlias, parameter.Value);
        }

        formattedQuery = formattedQuery.TrimEnd(',');

        return (formattedQuery, queryParams);
    }

    public void Dispose()
    {
        _sqlConnection.Dispose();
    }
}