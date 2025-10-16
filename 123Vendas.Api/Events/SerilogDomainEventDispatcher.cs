using _123Vendas.Domain.Events;
using _123Vendas.Domain.Events.Dispatcher;

namespace _123Vendas.Api.Events;

public sealed class SerilogDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<SerilogDomainEventDispatcher> _logger;

    public SerilogDomainEventDispatcher(ILogger<SerilogDomainEventDispatcher> logger)
        => _logger = logger;

    public Task PublicarEventoAsync(IEventDomain eventDomain, CancellationToken cancellationToken = default)
    {
        if (eventDomain == null)
            throw new ArgumentNullException(nameof(eventDomain));

        _logger.LogInformation(
            "[DomainEvent] {EventType} ocorreu em {DataOcorreuEm}. Detalhes: {@Evento}",
            eventDomain.GetType().Name,
            eventDomain.OcorreuEm,
            eventDomain);

        return Task.CompletedTask;
    }
}
