using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Services;
using Sqliste.Database.SqlServer.Extensions.ServiceCollection;
using Sqliste.Server.Middlewares;

namespace Sqliste.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddMemoryCache();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IHttpModelsFactory, HttpModelsFactory>();
        builder.Services.AddScoped<IDatabaseOpenApiService, DatabaseOpenApiService>();
        builder.Services.AddSqlServer(builder.Configuration);

        var app = builder.Build();

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

        app.UseAuthorization();

        app.UseMiddleware<DatabaseMiddleware>();
        app.MapControllers();

        //IWebSchemaEventDispatcher webSchemaEventDispatcher = app.Services.GetRequiredService<IWebSchemaEventDispatcher>();
        //webSchemaEventDispatcher.Init();

        IDatabaseEventWatcher databaseEventWatcher = app.Services.GetRequiredService<IDatabaseEventWatcher>();
        databaseEventWatcher.Init();

        await using(AsyncServiceScope scope = app.Services.CreateAsyncScope())
        {
            IDatabaseMigrationService databaseMigrationService = 
                scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
            databaseMigrationService.Migrate();

            IDatabaseIntrospectionService databaseIntrospectionService = 
                scope.ServiceProvider.GetRequiredService<IDatabaseIntrospectionService>();
            await databaseIntrospectionService.IntrospectAsync();
        }

        await app.RunAsync();
    }
}