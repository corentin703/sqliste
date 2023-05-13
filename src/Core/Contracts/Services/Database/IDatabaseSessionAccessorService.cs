namespace Sqliste.Core.Contracts.Services.Database;

public interface IDatabaseSessionAccessorService
{
    public Task<string?> GetSessionAsync(CancellationToken cancellationToken = default);
    public Task SetSessionAsync(string? databaseSessionJson, CancellationToken cancellationToken = default);
}