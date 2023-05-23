using System;
using System.Threading.Tasks;
using Coravel;
using Coravel.Queuing.Interfaces;
using DapperCodeFirstMappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sqliste.Core.Configuration;
using Sqliste.Core.Contracts.Services.Database;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Extensions.Host;
using Sqliste.Core.Extensions.ServiceCollection;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.SqlServer.Configuration;
using Sqliste.Database.SqlServer.Extensions.Host;
using Sqliste.Database.SqlServer.Extensions.ServiceCollection;
using Sqliste.Server.Middlewares;
using Sqliste.Server.Session;

namespace Sqliste.Server;

public class Program
{
    private const string AllowedOriginRuleName = "AllowedOrigins";
    
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

            // builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            // builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            
            DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(ProcedureModel).Assembly);
            DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(SqlServerConfiguration).Assembly);
            DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(Program).Assembly);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(x => x.GetRequiredService<ILogger<Program>>());

            builder.Services.Configure<SessionConfiguration>(builder.Configuration.GetSection("Session"));
            
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            builder.Services.AddDistributedMemoryCache();
            builder.Services
                .AddQueue()
                .AddScheduler()
            ;
            
            builder.Services.AddSession(options =>
            {
                SessionConfiguration sessionConfiguration = builder.Configuration.GetSection("Session").Get<SessionConfiguration>() ?? new();

                options.IdleTimeout = TimeSpan.FromMinutes(sessionConfiguration.IdleTimeout);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSqlisteCore();
            builder.Services.AddScoped<IDatabaseSessionAccessor, DatabaseSessionAccessor>();
            builder.Services.AddSqlServer(builder.Configuration);

            builder.Host.UseSerilog((context, services, configuration) =>
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} - {Level} ({SourceContext})] {Message}{NewLine}{Exception}")
            );

            WebApplication app = builder.Build();

            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/api/sqliste/Introspection/swagger.json", "SQListe");
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SQListe WS");
                });
            }

            // app.UseHttpsRedirection();
            // app.UseAuthorization();

            app.UseSession();

            string[]? allowedOrigins = app.Configuration.GetSection("AllowedOrigins").Get<string[]>();
            if (allowedOrigins != null)
            {
                app.UseCors(options =>
                {
                    options.WithOrigins(allowedOrigins);
                });
            }

            app.UseMiddleware<DatabaseMiddleware>();
            app.MapControllers();

            app.Services
                .ConfigureQueue()
                .OnError(exception =>
                {
                    app.Services
                        .GetRequiredService<ILogger<IQueue>>()
                        .LogError(exception: exception, "An error occurred during queued task");
                })
                .LogQueuedTaskProgress(app.Services.GetRequiredService<ILogger<IQueue>>());

            await app.RunMigrationsAsync();
            await app.RunInitialIntrospectionAsync();

            IDatabaseEventWatcher databaseEventWatcher = app.Services.GetRequiredService<IDatabaseEventWatcher>();
            databaseEventWatcher.Init();

            app.UseSqlServer();

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