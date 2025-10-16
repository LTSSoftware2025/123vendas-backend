using _123Vendas.Domain.Entities;
using System.Text.Json.Serialization;

namespace _123Vendas.Application.DTOs;

public class VendaDetalhesDtoResponse
{
    public Guid Id { get; set; }
    public string NumeroVenda { get; set; } = default!;
    public DateTime DataEfetuacao { get; set; }
    public Guid IdClienteExterno { get; set; }
    public string NomeCliente { get; set; } = default!;
    public Guid IdFilialExterna { get; set; }
    public string NomeFilial { get; set; } = default!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusVenda  Status { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemVendaDtoResponse> ItensVendaResponse { get; set; } = new();
}
