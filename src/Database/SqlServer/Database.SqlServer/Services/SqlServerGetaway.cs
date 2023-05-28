using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Utils;
using Sqliste.Database.Common.Contracts.Services;
using Sqliste.Database.SqlServer.Models;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

internal class SqlServerGetaway : IDatabaseGateway
{
    private readonly ILogger<SqlServerGetaway> _logger;
    private readonly IDatabaseQueryService _databaseQueryService;

    public SqlServerGetaway(ILogger<SqlServerGetaway> logger, IDatabaseQueryService databaseQueryService)
    {
        _logger = logger;
        _databaseQueryService = databaseQueryService;
    }

    public async Task<PipelineResponseBag> ExecProcedureAsync(PipelineRequestBag request, ProcedureModel procedure,
        Dictionary<string, object?> sqlParams,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug(
                "Starting exec for {ProcedureName} with {ParamCount} params", 
                procedure.Name,
                sqlParams.Count
            );
            List<PipelineResponseBag>? result = await _databaseQueryService.ExecAsync<PipelineResponseBag>(
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

            return result?.FirstOrDefault() ?? new PipelineResponseBag();
        }
        catch (SqlException sqlException)
        {
            _logger.LogError(exception: sqlException, "Error occurred during {Procedure} execution", procedure.Name);
            return new PipelineResponseBag()
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
        List<ProcedureModel>? procedures = await _databaseQueryService.QueryAsync<ProcedureModel>(query, args, cancellationToken: cancellationToken);

        if (procedures == null || procedures.Count == 0)
        {
            _logger.LogWarning("No procedures available");
            return new List<ProcedureModel>();
        }

        _logger.LogInformation("Queried {Number} procedures", procedures.Count);
        return procedures;
    }

    public async Task<List<ProcedureArgumentModel>> QueryProceduresParamsAsync(string procedureName, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Querying procedure params for {ProcedureName}", procedureName);
        (string query, object args) = IntrospectionSqlQueries.GetProceduresArgumentsQuery(procedureName);
        List<SqlServerProcedureArgumentModel>? rawProcedureArgs = await _databaseQueryService
            .QueryAsync<SqlServerProcedureArgumentModel>(query, args, cancellationToken);

        List<ProcedureArgumentModel> procedureArgs = rawProcedureArgs?.Select(arg => 
            new ProcedureArgumentModel()
            {
                Name = arg.Name,
                SqlDataType = arg.SqlDataType,
                IsSystemParam = SystemParamsUtils.GetAll().Contains(arg.Name), // arg.Name == "body" || arg.Name == "headers" || arg.Name == "cookies" || arg.Name == "data_bag",
            }
        ).ToList() ?? new List<ProcedureArgumentModel>();

        _logger.LogDebug("Got {Number} params for {ProcedureName}", procedureArgs.Count, procedureName);

        return procedureArgs;
    }
}