using _123Vendas.Domain.Events;
using _123Vendas.Domain.Events.Dispatcher;
using Microsoft.Extensions.Logging;

namespace _123Vendas.Infra.Events;

public class LoggerDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<LoggerDomainEventDispatcher> _logger;

    public LoggerDomainEventDispatcher(ILogger<LoggerDomainEventDispatcher> logger)
    {
        _logger = logger;
    }

    public Task PublicarEventoAsync(IEventDomain eventDomain, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Evento de domínio: {EventType} em {Timespan}",
            eventDomain.GetType().Name,
            eventDomain.OcorreuEm);

        return Task.CompletedTask;
    }
}
