namespace _123Vendas.Domain.Events;

public abstract record BaseEvent : IEventDomain
{
    public DateTime OcorreuEm { get; } = DateTime.UtcNow;
}