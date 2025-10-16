namespace _123Vendas.Application.DTOs;

public class CriarVendaDtoRequest
{
    public string NumeroVenda { get; set; } = default!;
    public DateTime DataEfetuacao { get; set; }
    public Guid IdClienteExterno { get; set; }
    public string NomeCliente { get; set; } = default!;
    public Guid IdFilialExterna { get; set; }
    public string NomeFilial { get; set; } = default!;
    public List<AdicionarItemVendaDtoRequest> ItensRequest { get; set; } = new();
}
