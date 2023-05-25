using Serilog;
using Sqliste.Database.Common.Extensions.Host;
using Sqliste.Infrastructure.Extensions.Host;
using Sqliste.Infrastructure.Extensions.ServiceCollection;
using Sqliste.Server.Extensions;
using Sqliste.Server.Extensions.WebApplicationExtensions;
using Sqliste.Server.Middlewares;

namespace Sqliste.Server;

public class Program
{
    public static async Task Main(string[] args)
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
            
            WebApplication app = builder.Build();

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
        catch (Exception exception)
        {
            Log.Fatal(exception: exception, "Application terminated unexpectedly");
        }
        finally
        {
            Log.Logger.Information("SQListe is terminating...");
            await Log.CloseAndFlushAsync();
        }
    }
}