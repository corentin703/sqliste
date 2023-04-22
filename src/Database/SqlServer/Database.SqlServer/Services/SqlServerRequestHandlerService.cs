using System.Net;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Services;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerRequestHandlerService : RequestHandlerService
{
    private readonly ILogger<SqlServerIntrospectionService> _logger;

    public SqlServerRequestHandlerService(IDatabaseIntrospectionService databaseIntrospectionService,
        IDatabaseService databaseService, ILogger<RequestHandlerService> logger,
        ILogger<SqlServerIntrospectionService> logger1) : base(databaseIntrospectionService, databaseService, logger)
    {
        _logger = logger1;
    }

    protected override async Task<HttpResponseModel> ExecRequestAsync(ProcedureModel procedure, Dictionary<string, object?> sqlParams, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting exec for {procedureName} with {paramCount} params", procedure.Name, sqlParams.Count);
        List<IDictionary<string, object>>? result =
            await DatabaseService.QueryAsync($"EXEC {procedure.Schema}.{procedure.Name}", sqlParams, cancellationToken);

        _logger.LogDebug("Ended exec for {procedureName} with {paramCount} params", procedure.Name, sqlParams.Count);
        return new HttpResponseModel()
        {
            Data = result,
            Status = HttpStatusCode.OK,
        };
    }
}