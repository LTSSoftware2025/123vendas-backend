namespace _123Vendas.Application.DTOs
{
    public class ItemVendaDtoRequest
    {
        public Guid IdProdutoExterno { get; set; }
        public string ProdutoDescricao { get; set; } = default!;
        public int Quantidade { get; set; }
        public decimal ValorUnitarioProduto { get; set; }
    }
}
