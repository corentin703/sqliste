using Serilog;

namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class LoggerExtensions
{
    public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(x => x.GetRequiredService<ILogger<Program>>());

        builder.Host.UseSerilog((context, services, configuration) =>
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} - {Level} ({SourceContext})] {Message}{NewLine}{Exception}")
        );
        
        return builder;
    }
}