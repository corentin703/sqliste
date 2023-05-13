using Microsoft.OpenApi.Models;

namespace Sqliste.Core.Contracts.Services;

public interface ISqlisteOpenApiService
{
    Task<string> GenerateOpenApiJsonAsync(CancellationToken cancellationToken = default);
}