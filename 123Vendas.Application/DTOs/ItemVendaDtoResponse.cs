namespace _123Vendas.Application.DTOs;

public class ItemVendaDtoResponse
{
    public Guid Id { get; set; }
    public Guid IdProdutoExterno { get; set; }
    public string ProdutoDescricao { get; set; } = default!;
    public int Quantidade { get; set; }
    public decimal ValorUnitarioProduto { get; set; }
    public decimal DescontoPercentual { get; set; }
    public decimal ValorTotalItem { get; set; }
}
