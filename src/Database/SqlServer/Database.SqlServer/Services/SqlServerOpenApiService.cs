using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Sqliste.Core.Contracts.Services.Database;
using Sqliste.Core.Models;
using Sqliste.Database.SqlServer.SqlQueries;

namespace Sqliste.Database.SqlServer.Services;

public class SqlServerOpenApiService : IDatabaseOpenApiService
{
    private readonly IDatabaseQueryService _databaseQueryService;
    private readonly ILogger<SqlServerOpenApiService> _logger;

    public SqlServerOpenApiService(IDatabaseQueryService databaseQueryService, ILogger<SqlServerOpenApiService> logger)
    {
        _databaseQueryService = databaseQueryService;
        _logger = logger;
    }

    public async Task<OpenApiDocument> GetDocumentFromDatabaseAsync(CancellationToken cancellationToken)
    {
        (string query, object args) = IntrospectionSqlQueries.GetOpenApiDocumentQuery();

        IDictionary<string, object>? result = (await _databaseQueryService.QueryAsync(query, args, cancellationToken))
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
                _logger.LogError("Error during OpenApi schema parsing : {Error} - {Pointer}", openApiError.Message, openApiError.Pointer);
            }

            foreach (OpenApiError openApiWarning in diagnostic.Warnings)
            {
                _logger.LogWarning("Warning during OpenApi schema parsing : {Error} - {Pointer}", openApiWarning.Message, openApiWarning.Pointer);
            }

            return document;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception: exception, "Can't parse OpenApi components");
            return new OpenApiDocument();
        }
    }

    public async Task<OpenApiTypeGetResponseModel> GetOpenApiTypeFromSqlTypeAsync(string sqlType, CancellationToken cancellationToken)
    {
        (string query, object args) = IntrospectionSqlQueries.GetOpenApiTypeFromSqlQuery(sqlType);
        List<OpenApiTypeGetResponseModel>? result = await _databaseQueryService.QueryAsync<OpenApiTypeGetResponseModel>(query, args, cancellationToken);

        return result?.FirstOrDefault() ?? new OpenApiTypeGetResponseModel()
        {
            Type = sqlType,
        };
    }
}