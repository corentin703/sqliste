﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Database.Common.Contracts.Services;
using Sqliste.Database.SqlServer.Configuration;
using Sqliste.Database.SqlServer.Jobs.Scheduling;
using Sqliste.Database.SqlServer.Services;

namespace Sqliste.Database.SqlServer.Extensions.ServiceCollection;

public static class AddSqlServerExtensions
{
    public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlServerConfiguration>(configuration.GetSection("Database"));

        services.AddSingleton<IDatabaseEventWatcher, SqlServerEventsWatcher>();

        services.AddScoped<IDatabaseQueryService, SqlServerDatabaseQueryService>();
        services.AddScoped<IDatabaseMigrationService, SqlServerMigrationService>();
        services.AddScoped<IDatabaseGateway, SqlServerGetaway>();
        services.AddScoped<IDatabaseOpenApiService, SqlServerOpenApiService>();

        services.AddTransient<DatabaseAppEventCleaningInvocable>();

        return services;
    }
}