namespace _123Vendas.Domain.Events;

public record CompraCanceladaEvent(Guid IdVenda, string NumeroVenda) : BaseEvent;
