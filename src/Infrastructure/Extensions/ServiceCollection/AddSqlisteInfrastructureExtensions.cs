using Microsoft.Extensions.DependencyInjection;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Jobs.Queuing;
using Sqliste.Infrastructure.Services;
using Sqliste.Infrastructure.Services.Events;

namespace Sqliste.Infrastructure.Extensions.ServiceCollection;

public static class AddSqlisteInfrastructureExtensions
{
    public static IServiceCollection AddSqlisteInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPipelineModelsFactory, PipelineModelsFactory>();
        services.AddScoped<IProcedureResolver, ProcedureResolver>();
        services.AddScoped<IParametersResolver, ParametersResolver>();
        services.AddScoped<IIntrospectionService, IntrospectionService>();
        services.AddScoped<IRequestHandler, RequestHandler>();
        services.AddScoped<IOpenApiGenerator, OpenApiGenerator>();
        services.AddTransient<IDatabaseEventDispatcher, DatabaseEventDispatcher>();
        services.AddTransient<IDatabaseEventHandlerResolver, DatabaseEventHandlerResolver>();
        services.AddTransient<DatabaseEventInvocable>();

        services.AddDatabaseEventHandlers();

        return services;
    }
}