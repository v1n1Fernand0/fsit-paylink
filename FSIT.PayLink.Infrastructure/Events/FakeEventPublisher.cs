using System.Text.Json;
using FSIT.PayLink.Application.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace FSIT.PayLink.Infrastructure.Events;

public class FakeEventPublisher : IEventPublisher
{
    private readonly ILogger<FakeEventPublisher> _log;
    public FakeEventPublisher(ILogger<FakeEventPublisher> log) => _log = log;

    public Task PublishAsync<TEvent>(TEvent evt, CancellationToken ct = default)
    {
        var payload = JsonSerializer.Serialize(evt);
        _log.LogInformation("[FAKE BUS] Published {EventType}: {Payload}",
                            typeof(TEvent).Name, payload);
        return Task.CompletedTask;
    }
}
