namespace _123Vendas.Domain.Events.Dispatcher;

public interface IDomainEventDispatcher
{
    Task PublicarEventoAsync(IEventDomain eventDomain, CancellationToken cancellationToken = default);
}
