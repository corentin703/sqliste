using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Services;
using Sqliste.Database.SqlServer.Models;
using System.Net;
using System.Net.Mime;
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

        if (rawResponse.Body is string stringBody)
            response.Body = ParseStringBody(stringBody, response);
        else 
            response.Body = rawResponse.Body;

        return response;
    }

    private object ParseStringBody(string stringBody, HttpResponseModel response)
    {
        string trimmedBody = stringBody.Trim();

        // If is body is JSON
        if ((trimmedBody.StartsWith("[") && trimmedBody.EndsWith("]")) || (trimmedBody.StartsWith("{") && trimmedBody.StartsWith("}")))
        {
            try
            {
                object? parsedJson = JsonSerializer.Deserialize<object>(trimmedBody);

                if (parsedJson != null)
                    response.Headers.TryAdd(HeaderNames.ContentType, MediaTypeNames.Application.Json);

                return parsedJson ?? stringBody;
            }
            catch
            {
                return stringBody;
            }
        }

        return stringBody;
    }
}