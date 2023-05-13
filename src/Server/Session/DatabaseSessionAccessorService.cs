using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Database;

namespace Sqliste.Server.Session;

public class DatabaseSessionAccessorService : IDatabaseSessionAccessorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<DatabaseSessionAccessorService> _logger;
    private const string DatabaseSessionKey = "sql_session";

    public DatabaseSessionAccessorService(IHttpContextAccessor httpContextAccessor, ILogger<DatabaseSessionAccessorService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<string?> GetSessionAsync(CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning("Trying to access to http session outside of HTTP Context");
            return null;
        }

        _logger.LogDebug("Accessing to http session");

        ISession session = _httpContextAccessor.HttpContext.Session;
        string? databaseSessionJson = session.GetString(DatabaseSessionKey);

        if (string.IsNullOrEmpty(databaseSessionJson))
        {
            _logger.LogInformation("Initializing http session");
            databaseSessionJson = "{}";
            session.SetString(DatabaseSessionKey, databaseSessionJson);
            await session.CommitAsync(cancellationToken);
        }

        return databaseSessionJson;
    }

    public async Task SetSessionAsync(string? databaseSessionJson, CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning("Trying to access to http session outside of HTTP Context");
            return;
        }
        
        _logger.LogDebug("Setting http session");
        
        ISession session = _httpContextAccessor.HttpContext.Session;

        databaseSessionJson = databaseSessionJson?.Trim();
        
        if (string.IsNullOrEmpty(databaseSessionJson))
            databaseSessionJson = "{}";
        
        session.SetString(DatabaseSessionKey, databaseSessionJson);
        await session.CommitAsync(cancellationToken);
    }
}