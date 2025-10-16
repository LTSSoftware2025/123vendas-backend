namespace _123Vendas.Domain.Events;

public record CompraEfetuadaEvent(Guid IdVenda, string NumeroVenda) : BaseEvent;
