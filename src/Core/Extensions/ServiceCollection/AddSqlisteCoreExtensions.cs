using Microsoft.Extensions.DependencyInjection;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Database;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Jobs.Queuing;
using Sqliste.Core.Services;
using Sqliste.Core.Services.Events;

namespace Sqliste.Core.Extensions.ServiceCollection;

public static class AddSqlisteCoreExtensions
{
    public static IServiceCollection AddSqlisteCore(this IServiceCollection services)
    {
        services.AddScoped<IHttpModelsFactory, HttpModelsFactory>();
        services.AddScoped<ISqlisteIntrospectionService, SqlisteIntrospectionService>();
        services.AddScoped<IRequestHandlerService, RequestHandlerService>();
        services.AddScoped<ISqlisteOpenApiService, SqlisteOpenApiService>();
        services.AddTransient<IDatabaseEventDispatcher, DatabaseEventDispatcher>();
        services.AddTransient<DatabaseEventInvocable>();

        return services;
    }
}