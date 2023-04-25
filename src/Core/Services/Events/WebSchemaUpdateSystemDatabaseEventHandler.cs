using Microsoft.Extensions.Logging;
using Sqliste.Core.Attributes.Events;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Models.Events;

namespace Sqliste.Core.Services.Events;

[SystemEventHandler("WebSchemaUpdate")]
public class WebSchemaUpdateSystemDatabaseEventHandler : IDatabaseEventHandler
{
    private readonly IDatabaseIntrospectionService _databaseIntrospectionService;
    private readonly ILogger<WebSchemaUpdateSystemDatabaseEventHandler> _logger;

    public WebSchemaUpdateSystemDatabaseEventHandler(IDatabaseIntrospectionService databaseIntrospectionService, ILogger<WebSchemaUpdateSystemDatabaseEventHandler> logger)
    {
        _databaseIntrospectionService = databaseIntrospectionService;
        _logger = logger;
    }

    public async Task Handle(EventModel model)
    {
        _logger.LogInformation("Detected change in web schema's procedures");
        _databaseIntrospectionService.Clear();
        await _databaseIntrospectionService.IntrospectAsync();
        _logger.LogInformation("Introspection done");
    }
}