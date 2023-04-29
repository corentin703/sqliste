using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models;
using Sqliste.Core.Services;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerOpenApiService : DatabaseOpenApiService
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<SqlServerOpenApiService> _logger;

    public SqlServerOpenApiService(
        ILogger<DatabaseOpenApiService> logger, 
        IDatabaseIntrospectionService databaseIntrospectionService, 
        IDatabaseService databaseService, 
        ILogger<SqlServerOpenApiService> logger1
    ) : base(logger, databaseIntrospectionService)
    {
        _databaseService = databaseService;
        _logger = logger1;
    }

    protected override async Task<OpenApiDocument> GetDocumentFromDatabaseAsync(CancellationToken cancellationToken)
    {
        (string query, object args) = IntrospectionSqlQueries.GetOpenApiDocumentQuery();

        IDictionary<string, object>? result = (await _databaseService.QueryAsync(query, args, cancellationToken))
            ?.FirstOrDefault();


        string? documentJson = null;
        if (result != null && result.TryGetValue("document", out object? value))
            documentJson = value as string;

        if (string.IsNullOrEmpty(documentJson))
            return new OpenApiDocument();

        OpenApiStringReader reader = new();

        try
        {
            OpenApiDocument document = reader.Read(documentJson, out OpenApiDiagnostic diagnostic);

            foreach (OpenApiError openApiError in diagnostic.Errors)
            {
                _logger.LogError("Error during OpenApi schema parsing : {error} - {pointer}", openApiError.Message, openApiError.Pointer);
            }

            foreach (OpenApiError openApiWarning in diagnostic.Warnings)
            {
                _logger.LogWarning("Warning during OpenApi schema parsing : {error} - {pointer}", openApiWarning.Message, openApiWarning.Pointer);
            }

            return document;
        }
        catch (Exception exception)
        {
            _logger.LogError("Can't parse OpenApi components : returning empty one {exception}", exception);
            return new OpenApiDocument();
        }
    }

    protected override async Task<OpenApiTypeGetResponseModel> GetOpenApiTypeFromSqlTypeAsync(string sqlType, CancellationToken cancellationToken)
    {
        (string query, object args) = IntrospectionSqlQueries.GetOpenApiTypeFromSqlQuery(sqlType);
        List<OpenApiTypeGetResponseModel>? result = await _databaseService.QueryAsync<OpenApiTypeGetResponseModel>(query, args, cancellationToken);

        return result?.FirstOrDefault() ?? new OpenApiTypeGetResponseModel()
        {
            Type = sqlType,
        };
    }
}