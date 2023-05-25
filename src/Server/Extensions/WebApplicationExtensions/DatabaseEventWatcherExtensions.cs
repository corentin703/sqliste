using Sqliste.Core.Contracts.Services.Events;

namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class DatabaseEventWatcherExtensions
{
    public static WebApplication UseDatabaseEventWatcher(this WebApplication app)
    {
        IDatabaseEventWatcher databaseEventWatcher = app.Services.GetRequiredService<IDatabaseEventWatcher>();
        databaseEventWatcher.Init();
        
        return app;
    }
}