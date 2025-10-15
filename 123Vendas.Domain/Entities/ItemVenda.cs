namespace _123Vendas.Domain.Entities;

public sealed class ItemVenda
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid IdProdutoExterno { get; private set; }
    public string ProdutoDescricao { get; private set; } = default!;
    public int Quantidade { get; private set; }
    public decimal ValorUnitarioProduto { get; private set; }
    public decimal DescontoPercentual { get; private set; }

    public decimal ValorTotalItem => 
        Math.Round((ValorUnitarioProduto * Quantidade) * (1 - (DescontoPercentual / 100m)), 2);

    private ItemVenda()
    {
    }

    /// <summary>
    /// Cria um item de venda, garantindo que os dados sejam válidos
    /// </summary>
    /// <param name="idProdutoExterno"></param>
    /// <param name="produtoDescricao"></param>
    /// <param name="quantidade"></param>
    /// <param name="valorUnitarioProduto"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private ItemVenda(Guid idProdutoExterno, string produtoDescricao, int quantidade, decimal valorUnitarioProduto)
    {
        if (quantidade <=0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior do que 0.");

        if (quantidade > 20)
            throw new InvalidOperationException("Não é possível vender acima de 20 itens iguais.");

        if (valorUnitarioProduto <= 0)
            throw new ArgumentOutOfRangeException(nameof(valorUnitarioProduto), "O valor do produto deve ser maior do que 0.");

        if (string.IsNullOrWhiteSpace(produtoDescricao))
            throw new ArgumentException("A descrição do produto é obrigatória.", nameof(produtoDescricao));

        IdProdutoExterno = idProdutoExterno;
        ProdutoDescricao = produtoDescricao;
        Quantidade = quantidade;
        ValorUnitarioProduto = valorUnitarioProduto;
        DescontoPercentual = RegrasDesconto.CalcularDesconto(quantidade);
    }

    public static ItemVenda Criar(Guid idProdutoExterno, string produtoDescricao, int quantidade, decimal valorUnitarioProduto)
        => new(idProdutoExterno, produtoDescricao, quantidade, valorUnitarioProduto);
}
