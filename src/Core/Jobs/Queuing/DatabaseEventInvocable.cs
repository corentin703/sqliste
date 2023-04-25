using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services.Events;
using Sqliste.Core.Models.Events;

namespace Sqliste.Core.Jobs.Queuing;

public class DatabaseEventInvocablePayload
{
    public List<EventModel> Events { get; set; } = new();
}

public class DatabaseEventInvocable : IInvocable, IInvocableWithPayload<DatabaseEventInvocablePayload>
{
    private readonly ILogger<DatabaseEventInvocable> _logger;
    private readonly IDatabaseEventDispatcher _databaseEventDispatcher;


    public DatabaseEventInvocable(ILogger<DatabaseEventInvocable> logger, IDatabaseEventDispatcher databaseEventDispatcher)
    {
        _logger = logger;
        _databaseEventDispatcher = databaseEventDispatcher;
    }

    public DatabaseEventInvocablePayload Payload { get; set; } = new();

    public async Task Invoke()
    {
        try
        {
            _logger.LogInformation("Received events from database : triggering");
            await _databaseEventDispatcher.DispatchEventsAsync(Payload.Events);
            _logger.LogInformation("Database events processed");
        }
        catch (Exception exception)
        {
            _logger.LogError("An error occurred during event dispatching : {error}", exception.ToString());
        }
    }
}