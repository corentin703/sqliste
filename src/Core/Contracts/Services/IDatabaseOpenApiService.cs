using Microsoft.OpenApi.Models;

namespace Sqliste.Core.Contracts.Services;

public interface IDatabaseOpenApiService
{
    Task<string> GenerateOpenApiJsonAsync(CancellationToken cancellationToken = default);
}