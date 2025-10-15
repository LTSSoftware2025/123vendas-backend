namespace _123Vendas.Domain.Events;

public record ItemCanceladoEvent(Guid IdVenda, Guid IdItem) : BaseEvent;
