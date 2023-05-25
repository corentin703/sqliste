using Microsoft.OpenApi.Models;
using Sqliste.Core.Models;

namespace Sqliste.Database.Common.Contracts.Services;

public interface IDatabaseOpenApiService
{
    public Task<OpenApiDocument> GetDocumentFromDatabaseAsync(CancellationToken cancellationToken);

    public Task<OpenApiTypeGetResponseModel> GetOpenApiTypeFromSqlTypeAsync(string sqlType, CancellationToken cancellationToken);
}