using System.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Services;
using Sqliste.Database.SqlServer.Models;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerIntrospectionService : DatabaseIntrospectionService
{
    private readonly ILogger<SqlServerIntrospectionService> _logger;

    public SqlServerIntrospectionService(IDatabaseService databaseService, IMemoryCache memoryCache,
        ILogger<SqlServerIntrospectionService> logger) 
        : base(databaseService, memoryCache, logger)
    {
        _logger = logger;
    }

    protected override async Task<List<ProcedureModel>> QueryProceduresAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Querying procedures");
        (string query, object args) = IntrospectionSqlQueries.GetProceduresQuery();
        List<ProcedureModel>? procedures = await DatabaseService.QueryAsync<ProcedureModel>(query, args, cancellationToken: cancellationToken);

        if (procedures == null || procedures.Count == 0)
        {
            _logger.LogWarning("No procedures available");
            return new List<ProcedureModel>();
        }

        _logger.LogInformation("Queried {number}", procedures.Count);
        return procedures;
    }

    protected override async Task<List<ProcedureArgumentModel>> QueryProceduresParamsAsync(string procedureName, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Querying procedure params for {procedureName}", procedureName);
        (string query, object args) = IntrospectionSqlQueries.GetProceduresArgumentsQuery(procedureName);
        List<SqlServerProcedureArgumentModel>? rawProcedureArgs = await DatabaseService
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

        _logger.LogInformation("Got {number} params for {procedureName}", procedureArgs.Count, procedureName);

        return procedureArgs;
    }
}