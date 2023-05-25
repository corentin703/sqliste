using Coravel;
using Coravel.Queuing.Interfaces;

namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class CoravelExtensions
{
    public static WebApplicationBuilder AddCoravel(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddQueue()
            .AddScheduler()
        ;

        return builder;
    }

    public static WebApplication UseCoravel(this WebApplication app)
    {
        app.Services
            .ConfigureQueue()
            .OnError(exception =>
            {
                app.Services
                    .GetRequiredService<ILogger<IQueue>>()
                    .LogError(exception: exception, "An error occurred during queued task");
            })
            .LogQueuedTaskProgress(app.Services.GetRequiredService<ILogger<IQueue>>());

        return app;
    }
}