using Sqliste.Core.Configuration;

namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class SwaggerExtensions
{
    public static WebApplicationBuilder AddSqlisteSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<SessionConfiguration>(builder.Configuration.GetSection("Session"));
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        return builder;
    }

    public static WebApplication UseSqlisteSwagger(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
            return app;
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/api/sqliste/Introspection/swagger.json", "SQListe");
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "SQListe WS");
        });
        
        return app;
    }
}