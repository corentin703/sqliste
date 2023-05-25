namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class CorsExtensions
{
    public static WebApplication UseCors(this WebApplication app)
    {
        string[]? allowedOrigins = app.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        if (allowedOrigins != null)
        {
            app.UseCors(options =>
            {
                options.WithOrigins(allowedOrigins);
            });
        }

        return app;
    }
}