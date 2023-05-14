using Coravel;
using Coravel.Queuing.Interfaces;
using DapperCodeFirstMappings;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Database;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Extensions.ServiceCollection;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.SqlServer.Configuration;
using Sqliste.Database.SqlServer.Extensions.Host;
using Sqliste.Database.SqlServer.Extensions.ServiceCollection;
using Sqliste.Server.Extensions.ServiceCollection;
using Sqliste.Server.Middlewares;
using Sqliste.Server.Session;

namespace Sqliste.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(ProcedureModel).Assembly);
        DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(SqlServerConfiguration).Assembly);
        DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(Program).Assembly);

        builder.Services.AddHttpContextAccessor();

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
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddSqlisteCore();
        builder.Services.AddScoped<IDatabaseSessionAccessorService, DatabaseSessionAccessorService>();
        builder.Services.AddSqlServer(builder.Configuration);
        
        builder.Services.AddDatabaseEventHandlers();

        WebApplication app = builder.Build();

        app.UseHttpLogging();
        
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

        app.UseHttpsRedirection();

        // app.UseAuthorization();

        app.UseSession();
        
        app.UseMiddleware<DatabaseMiddleware>();
        app.MapControllers();

        app.Services
            .ConfigureQueue()
            .OnError(exception =>
            {
                app.Services
                    .GetRequiredService<ILogger<IQueue>>()
                    .LogError("An error occurred during queued task : {exception}", exception.ToString()); 
            })
            .LogQueuedTaskProgress(app.Services.GetRequiredService<ILogger<IQueue>>());

        await using(AsyncServiceScope scope = app.Services.CreateAsyncScope())
        {
            IDatabaseMigrationService databaseMigrationService = 
                scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
            // databaseMigrationService.Migrate();

            ISqlisteIntrospectionService sqlisteIntrospectionService =
                scope.ServiceProvider.GetRequiredService<ISqlisteIntrospectionService>();
            await sqlisteIntrospectionService.IntrospectAsync();
        }

        IDatabaseEventWatcher databaseEventWatcher = app.Services.GetRequiredService<IDatabaseEventWatcher>();
        databaseEventWatcher.Init();

        app.UseSqlServer();

        await app.RunAsync();
    }
}