namespace Sqliste.Database.Common.Contracts.Services;

public interface IDatabaseSessionAccessor
{
    public Task<string?> GetSessionAsync(CancellationToken cancellationToken = default);
    public Task SetSessionAsync(string? databaseSessionJson, CancellationToken cancellationToken = default);
}