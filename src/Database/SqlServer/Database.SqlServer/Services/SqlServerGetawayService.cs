using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services.Database;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.SqlServer.Models;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerGetawayService : IDatabaseGatewayService
{
    private readonly ILogger<SqlServerGetawayService> _logger;
    private readonly IDatabaseService _databaseService;

    public SqlServerGetawayService(ILogger<SqlServerGetawayService> logger, IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    public async Task<HttpRequestModel?> ExecProcedureAsync(HttpRequestModel request, ProcedureModel procedure, Dictionary<string, object?> sqlParams,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug(
                "Starting exec for {ProcedureName} with {ParamCount} params", 
                procedure.Name,
                sqlParams.Count
            );
            List<HttpRequestModel>? result = await _databaseService.ExecAsync<HttpRequestModel>(
                $"{procedure.Schema}.{procedure.Name}",
                procedure.Arguments,
                sqlParams,
                cancellationToken
            );

            _logger.LogDebug(
                "Ended exec for {ProcedureName} with {ParamCount} params", 
                procedure.Name, 
                sqlParams.Count
            );

            return result?.FirstOrDefault();
        }
        catch (SqlException sqlException)
        {
            _logger.LogError(exception: sqlException, "Error occurred during {Procedure} execution", procedure.Name);
            return new HttpRequestModel()
            {
                Error = new SqlErrorModel() 
                {
                    Message = sqlException.Message,
                    Attributes = new Dictionary<string, object>()
                    {
                        {"state", sqlException.State},
                    },
                    RawException = sqlException,
                },
            };
        }
    }

    public async Task<List<ProcedureModel>> QueryProceduresAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Querying procedures");
        (string query, object args) = IntrospectionSqlQueries.GetProceduresQuery();
        List<ProcedureModel>? procedures = await _databaseService.QueryAsync<ProcedureModel>(query, args, cancellationToken: cancellationToken);

        if (procedures == null || procedures.Count == 0)
        {
            _logger.LogWarning("No procedures available");
            return new List<ProcedureModel>();
        }

        _logger.LogInformation("Queried {Number}", procedures.Count);
        return procedures;
    }

    public async Task<List<ProcedureArgumentModel>> QueryProceduresParamsAsync(string procedureName, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Querying procedure params for {ProcedureName}", procedureName);
        (string query, object args) = IntrospectionSqlQueries.GetProceduresArgumentsQuery(procedureName);
        List<SqlServerProcedureArgumentModel>? rawProcedureArgs = await _databaseService
            .QueryAsync<SqlServerProcedureArgumentModel>(query, args, cancellationToken);

        List<ProcedureArgumentModel> procedureArgs = rawProcedureArgs?.Select(arg => 
            new ProcedureArgumentModel()
            {
                Name = arg.Name,
                SqlDataType = arg.SqlDataType,
                Direction = arg.IsOutput ? ParameterDirection.InputOutput : ParameterDirection.Input,
                IsSystemParam = arg.Name == "body" || arg.Name == "headers" || arg.Name == "cookies" || arg.Name == "data_bag",
            }
        ).ToList() ?? new List<ProcedureArgumentModel>();

        _logger.LogInformation("Got {Number} params for {ProcedureName}", procedureArgs.Count, procedureName);

        return procedureArgs;
    }
}