using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Services;
using Sqliste.Database.SqlServer.Models;
using System.Net;
using System.Text.Json;

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
        List<SqlServerHttpResponseModel>? result = await DatabaseService.QueryAsync<SqlServerHttpResponseModel>($"EXEC {procedure.Schema}.{procedure.Name}", sqlParams, cancellationToken);

        _logger.LogDebug("Ended exec for {procedureName} with {paramCount} params", procedure.Name, sqlParams.Count);

        SqlServerHttpResponseModel? rawResponse = result?.FirstOrDefault();

        if (rawResponse == null)
        {
            return new HttpResponseModel()
            {
                Status = HttpStatusCode.NoContent,
            };
        }

        return ConvertToStandardResponseModel(rawResponse);
    }

    private HttpResponseModel ConvertToStandardResponseModel(SqlServerHttpResponseModel rawResponse)
    {
        HttpResponseModel response = new()
        {
            Status = rawResponse.Status,
            Body = rawResponse.Body?.Trim(),
        };

        if (!string.IsNullOrEmpty(rawResponse.Headers))
        {
            try
            {
                response.Headers =
                    JsonSerializer.Deserialize<Dictionary<string, string>>(rawResponse.Headers) ?? new();
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Failed to parse headers : {exception}", exception.Message);
                response.Headers = new();
            }
        }

        return response;
    }
}