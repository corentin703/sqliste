using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.SqlServer.Configuration;
using System.Data;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerDatabaseService : IDatabaseService, IDisposable
{
    private readonly SqlConnection _sqlConnection;
    private readonly ILogger<SqlServerDatabaseService> _logger;

    public SqlServerDatabaseService(IOptionsSnapshot<SqlServerConfiguration> configuration, ILogger<SqlServerDatabaseService> logger)
    {
        _logger = logger;
        _sqlConnection = new SqlConnection(configuration.Value.ConnectionString);
    }

    public async Task<List<IDictionary<string, object>>?> QueryAsync(
        string query, 
        Dictionary<string, object?> parameters, 
        CancellationToken cancellationToken = default
    )
    {
        (string formattedQuery, DynamicParameters queryParams) = EscapeParams(query, parameters);

        _logger.LogDebug("Running query {query} with {paramCount} args", query, parameters.Count);
        IEnumerable<dynamic>? result = await _sqlConnection.QueryAsync(formattedQuery, queryParams);
        return result?.Cast<IDictionary<string, object>>().ToList();
    }

    public async Task<List<T>?> QueryAsync<T>(
        string query, 
        Dictionary<string, object?> parameters, 
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogDebug("Running query {query} with {paramCount} args", query, parameters.Count);
        (string formattedQuery, DynamicParameters queryParams) = EscapeParams(query, parameters);
        return (await _sqlConnection.QueryAsync<T>(formattedQuery, queryParams))?.ToList();
    }

    public async Task<List<IDictionary<string, object>>?> QueryAsync(
        string query, 
        object? parameters = null,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogDebug("Running query {query}", query);
        IEnumerable<dynamic>? result = await _sqlConnection.QueryAsync(query, parameters);
        return result?.Cast<IDictionary<string, object>>().ToList();
    }

    public async Task<List<T>?> QueryAsync<T>(
        string query, 
        object? parameters = null,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogDebug("Running query {query}", query);
        return (await _sqlConnection.QueryAsync<T>(query, parameters))?.ToList();
    }

    public async Task<List<T>?> ExecAsync<T>(
        string procedure, 
        List<ProcedureArgumentModel> parameters, 
        Dictionary<string, object?> parameterValues,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogDebug("Running procedure {query} with {paramsCount}", procedure, parameterValues.Count);

        string query = $"EXEC {procedure}";

        //List<ProcedureArgumentModel> outputParams = new();
        DynamicParameters queryParams = new();
        foreach (ProcedureArgumentModel parameter in parameters)
        {
            object? value = null;
            if (!parameterValues.TryGetValue(parameter.Name, out value))
            {
                _logger.LogInformation("Ignoring param {paramName}", parameter.Name);
                continue;
            }

            _logger.LogDebug("Param {paramName} added", parameter.Name);
            string escapingAlias = $"{parameter.Name}_ESC";
            queryParams.Add(escapingAlias, value, direction: parameter.Direction);
            query = $"{query} @{parameter.Name} = @{escapingAlias},";

            //if (parameter.Direction is ParameterDirection.InputOutput or ParameterDirection.Output)
            //    outputParams.Add(parameter);
        }

        query = query.TrimEnd(',');

        List<T>? result = (await _sqlConnection.QueryAsync<T>(query, queryParams))?.ToList();

        //MethodInfo getOutputParamMethod = typeof(DynamicParameters).GetMethod("Get")!;
        //outputParams.ForEach(param =>
        //{
        //    MethodInfo method = getOutputParamMethod.MakeGenericMethod(parameterValues[param.Name]?.GetType() ?? typeof(string));
        //    parameterValues[param.Name] = method.Invoke(queryParams, new []{ param.Name });
        //});

        return result;
    }

    private (string, DynamicParameters) EscapeParams(
        string baseQuery, 
        Dictionary<string, object?> parameters 
    )
    {
        string formattedQuery = baseQuery;
        DynamicParameters queryParams = new();

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