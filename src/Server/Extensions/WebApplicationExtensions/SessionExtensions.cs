using Sqliste.Core.Configuration;
using Sqliste.Database.Common.Contracts.Services;
using Sqliste.Server.Session;

namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class SessionExtensions
{
    public static WebApplicationBuilder AddSqlisteSession(this WebApplicationBuilder builder)
    {
        builder.Services.AddSession(options =>
        {
            SessionConfiguration sessionConfiguration = builder.Configuration.GetSection("Session").Get<SessionConfiguration>() ?? new();

            options.IdleTimeout = TimeSpan.FromMinutes(sessionConfiguration.IdleTimeout);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
        builder.Services.AddScoped<IDatabaseSessionAccessor, DatabaseSessionAccessor>();
        
        return builder;
    }

    public static WebApplication UseSqlisteSession(this WebApplication app)
    {
        app.UseSession();

        return app;
    }
}