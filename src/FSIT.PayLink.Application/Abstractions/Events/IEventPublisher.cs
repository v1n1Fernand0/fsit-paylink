namespace FSIT.PayLink.Application.Abstractions.Events;

/// <summary>Publica eventos de domínio/integration na infraestrutura de fila.</summary>
public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default);
}
