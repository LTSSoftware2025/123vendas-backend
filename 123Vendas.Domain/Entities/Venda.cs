using _123Vendas.Domain.Events;

namespace _123Vendas.Domain.Entities;

public sealed class Venda
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string NumeroVenda { get; private set; } = default!;
    public DateTime DataEfetuacao { get; private set; }
    public bool Cancelada { get; private set; }
    public StatusVenda Status => Cancelada ? StatusVenda.Cancelada : StatusVenda.NaoCancelado;
    public Guid IdClienteExterno { get; private set; }
    public string NomeCliente { get; private set; } = default!;
    public Guid IdFilialExterna { get; private set; }
    public string NomeFilial { get; private set; } = default!;
    private readonly List<ItemVenda> _itensDaVenda = new();
    public IReadOnlyCollection<ItemVenda> ItensDaVenda => _itensDaVenda.AsReadOnly();
    public decimal ValorTotal => Math.Round(_itensDaVenda.Sum(i => i.ValorTotalItem), 2);

    private readonly List<IEventDomain> _eventos = new();
    public IReadOnlyCollection<IEventDomain> Eventos => _eventos.AsReadOnly();
    public void LimparEventos() => _eventos.Clear();

    private Venda()
    {
    }

    /// <summary>
    /// Cria uma nova venda, garantindo que os dados sejam válidos
    /// </summary>
    /// <param name="numeroVenda"></param>
    /// <param name="dataEfetuacao"></param>
    /// <param name="idClienteExterno"></param>
    /// <param name="nomeCliente"></param>
    /// <param name="idFilialExterna"></param>
    /// <param name="nomeFilial"></param>
    /// <exception cref="ArgumentException"></exception>
    private Venda(string numeroVenda, DateTime dataEfetuacao, Guid idClienteExterno, string nomeCliente, Guid idFilialExterna, string nomeFilial)
    {
        if (string.IsNullOrWhiteSpace(numeroVenda))
            throw new ArgumentException("O número da venda é obrigatório.", nameof(numeroVenda));

        if (string.IsNullOrWhiteSpace(nomeCliente))
            throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nomeCliente));

        if (string.IsNullOrWhiteSpace(nomeFilial))
            throw new ArgumentException("O nome da filial é obrigatório.", nameof(nomeFilial));

        NumeroVenda = numeroVenda.Trim();
        DataEfetuacao = dataEfetuacao;
        IdClienteExterno = idClienteExterno;
        NomeCliente = nomeCliente.Trim();
        IdFilialExterna = idFilialExterna;
        NomeFilial = nomeFilial.Trim();
    }

    public static Venda CriarNova(string numeroVenda, DateTime dataEfetuacao, Guid idClienteExterno, string nomeCliente, Guid idFilialExterna, string nomeFilial)
        => new(numeroVenda, dataEfetuacao, idClienteExterno, nomeCliente, idFilialExterna, nomeFilial);

    public ItemVenda AdicionarItem(Guid idProduto, string produtoDescricao, int quantidade, decimal valorUnitarioProduto)
    {
        GarantirVendaNaoCancelada();

        var itemDaVenda = ItemVenda.Criar(idProduto, produtoDescricao, quantidade, valorUnitarioProduto);
        _itensDaVenda.Add(itemDaVenda);

        _eventos.Add(new ItemAdicionadoEvent(Id, itemDaVenda.Id));

        return itemDaVenda;
    }

    public void RemoverItemDaVenda(Guid idItem)
    {
        GarantirVendaNaoCancelada();

        var itemDaVenda = _itensDaVenda.FirstOrDefault(i => i.Id == idItem);

        if (itemDaVenda is null)
            throw new InvalidOperationException("O item não foi encontrado!");

        _itensDaVenda.Remove(itemDaVenda);

        _eventos.Add(new ItemCanceladoEvent(Id, idItem));
    }

    public void AtualizarDados(string nomeCliente, string nomeFilial)
    {
        GarantirVendaNaoCancelada();

        if (string.IsNullOrWhiteSpace(nomeCliente))
            throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nomeCliente));

        if (string.IsNullOrWhiteSpace(nomeFilial))
            throw new ArgumentException("O nome da filial é obrigatório.", nameof(nomeFilial));

        NomeCliente = nomeCliente.Trim();
        NomeFilial = nomeFilial.Trim();
    }

    public void Cancelar()
    {
        if (Cancelada)
            return;

        Cancelada = true;

        _eventos.Add(new CompraCanceladaEvent(Id, NumeroVenda));
    }

    private void GarantirVendaNaoCancelada()
    {
        if (Cancelada)
            throw new InvalidOperationException("Não é permitido realizar alterações em uma venda cancelada.");
    }
}
