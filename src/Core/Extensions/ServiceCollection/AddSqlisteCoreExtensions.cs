using Microsoft.Extensions.DependencyInjection;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Jobs.Queuing;
using Sqliste.Core.Services;
using Sqliste.Core.Services.Events;

namespace Sqliste.Core.Extensions.ServiceCollection;

public static class AddSqlisteCoreExtensions
{
    public static IServiceCollection AddSqlisteCore(this IServiceCollection services)
    {
        services.AddScoped<IPipelineModelsFactory, PipelineModelsFactory>();
        services.AddScoped<IProcedureResolver, ProcedureResolver>();
        services.AddScoped<ISqlisteIntrospectionService, SqlisteIntrospectionService>();
        services.AddScoped<IRequestHandler, RequestHandler>();
        services.AddScoped<ISqlisteOpenApiGenerator, SqlisteOpenApiGenerator>();
        services.AddTransient<IDatabaseEventDispatcher, DatabaseEventDispatcher>();
        services.AddTransient<IDatabaseEventHandlerResolver, DatabaseEventHandlerResolver>();
        services.AddTransient<DatabaseEventInvocable>();

        services.AddDatabaseEventHandlers();

        return services;
    }
}