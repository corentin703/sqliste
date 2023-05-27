namespace Sqliste.Core.Contracts.Services;

public interface IOpenApiGenerator
{
    Task<string> GenerateOpenApiJsonAsync(CancellationToken cancellationToken = default);
}