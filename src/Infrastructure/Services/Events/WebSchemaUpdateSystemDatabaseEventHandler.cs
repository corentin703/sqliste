using Microsoft.Extensions.Logging;
using Sqliste.Core.Attributes.Events;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Models.Events;

namespace Sqliste.Infrastructure.Services.Events;

[SystemEventHandler("WebSchemaUpdate")]
internal class WebSchemaUpdateSystemDatabaseEventHandler : IDatabaseEventHandler
{
    private readonly IIntrospectionService _introspectionService;
    private readonly ILogger<WebSchemaUpdateSystemDatabaseEventHandler> _logger;

    public WebSchemaUpdateSystemDatabaseEventHandler(IIntrospectionService introspectionService, ILogger<WebSchemaUpdateSystemDatabaseEventHandler> logger)
    {
        _introspectionService = introspectionService;
        _logger = logger;
    }

    public async Task Handle(EventModel model)
    {
        _logger.LogInformation("Detected change in web schema's procedures");
        _introspectionService.Clear();
        await _introspectionService.IntrospectAsync();
        _logger.LogInformation("Introspection done");
    }
}