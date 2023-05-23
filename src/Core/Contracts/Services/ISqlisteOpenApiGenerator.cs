using Microsoft.OpenApi.Models;

namespace Sqliste.Core.Contracts.Services;

public interface ISqlisteOpenApiGenerator
{
    Task<string> GenerateOpenApiJsonAsync(CancellationToken cancellationToken = default);
}