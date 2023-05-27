using Serilog;
using Sqliste.Database.Common.Extensions.Host;
using Sqliste.Infrastructure.Extensions.Host;
using Sqliste.Infrastructure.Extensions.ServiceCollection;
using Sqliste.Server.Cli;
using Sqliste.Server.Extensions;
using Sqliste.Server.Extensions.CliExtensions;
using Sqliste.Server.Extensions.WebApplicationExtensions;
using Sqliste.Server.Middlewares;

namespace Sqliste.Server;

public class Program
{
    public static async Task<int> Main(params string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        
        try
        {
            Log.Logger.Information("SQListe is starting up !");
            
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.AddLogger();
            DapperExtensions.LoadMappings();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            builder.AddCoravel();

            builder.AddSqlisteCache();
            builder.AddSqlisteSession();
            builder.AddSqlisteSwagger();

            builder.Services.AddSqlisteInfrastructure();
            builder.AddDatabaseConnector();

            builder.Services.AddCliServices();

            WebApplication app = builder.Build();

            if (args.Length == 0)
                await RunServer(app);
            else
                await RunCli(app, args);
            
            return 0;
        }
        catch (Exception exception)
        {
            Log.Logger.Fatal(exception: exception, "Application terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.Logger.Information("SQListe is terminating...");
            await Log.CloseAndFlushAsync();
        }
    }

    private static async Task RunCli(WebApplication app, params string[] args)
    {
        await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
        CliApplication cli = scope.ServiceProvider.GetRequiredService<CliApplication>();
        await cli.RunAsync(args);
    }

    private static async Task RunServer(WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.UseCoravel();
        app.UseSqlisteSwagger();
        app.UseSqlisteSession();
        app.UseCors();

        app.UseMiddleware<DatabaseMiddleware>();
        app.MapControllers();

        await app.RunMigrationsAsync();
        await app.RunInitialIntrospectionAsync();

        app.UseDatabaseEventWatcher();
        app.UseDatabaseConnector();
            
        await app.RunAsync();
    }
}