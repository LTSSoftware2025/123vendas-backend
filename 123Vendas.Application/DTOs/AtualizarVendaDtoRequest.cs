namespace _123Vendas.Application.DTOs;

public class AtualizarVendaDtoRequest
{
    public Guid Id { get; set; }
    public string NomeCliente { get; set; } = default!;
    public string NomeFilial { get; set; } = default!;
}
