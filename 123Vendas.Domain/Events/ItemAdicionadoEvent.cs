namespace _123Vendas.Domain.Events;

public record ItemAdicionadoEvent(Guid IdVenda, Guid IdItem) : BaseEvent;
