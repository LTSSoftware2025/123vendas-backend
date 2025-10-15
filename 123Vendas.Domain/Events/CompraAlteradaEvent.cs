namespace _123Vendas.Domain.Events;

public record CompraAlteradaEvent(Guid IdVenda, string NumeroVenda) : BaseEvent;
