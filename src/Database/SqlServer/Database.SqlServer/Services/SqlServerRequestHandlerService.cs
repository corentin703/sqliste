using Microsoft.Data.SqlClient;
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

    protected override async Task<HttpRequestModel?> ExecProcedureAsync(
        HttpRequestModel request,
        ProcedureModel procedure, 
        Dictionary<string, object?> sqlParams, 
        CancellationToken cancellationToken
    )
    {
        try
        {
            _logger.LogDebug("Starting exec for {procedureName} with {paramCount} params", procedure.Name,
                sqlParams.Count);
            List<HttpRequestModel>? result = await DatabaseService.ExecAsync<HttpRequestModel>(
                $"{procedure.Schema}.{procedure.Name}",
                procedure.Arguments,
                sqlParams,
                cancellationToken
            );

            _logger.LogDebug("Ended exec for {procedureName} with {paramCount} params", procedure.Name,
                sqlParams.Count);

            return result?.FirstOrDefault();
        }
        catch (SqlException sqlException)
        {
            _logger.LogError("Error occurred during {procedure} execution : {error}", procedure.Name, sqlException);
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
}