using Microsoft.Extensions.Logging;
using Sqliste.Core.Attributes.Events;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Models.Events;

namespace Sqliste.Core.Services.Events;

[SystemEventHandler("WebSchemaUpdate")]
public class WebSchemaUpdateSystemDatabaseEventHandler : IDatabaseEventHandler
{
    private readonly ISqlisteIntrospectionService _sqlisteIntrospectionService;
    private readonly ILogger<WebSchemaUpdateSystemDatabaseEventHandler> _logger;

    public WebSchemaUpdateSystemDatabaseEventHandler(ISqlisteIntrospectionService sqlisteIntrospectionService, ILogger<WebSchemaUpdateSystemDatabaseEventHandler> logger)
    {
        _sqlisteIntrospectionService = sqlisteIntrospectionService;
        _logger = logger;
    }

    public async Task Handle(EventModel model)
    {
        _logger.LogInformation("Detected change in web schema's procedures");
        _sqlisteIntrospectionService.Clear();
        await _sqlisteIntrospectionService.IntrospectAsync();
        _logger.LogInformation("Introspection done");
    }
}