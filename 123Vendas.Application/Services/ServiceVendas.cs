using _123Vendas.Application.DTOs;
using _123Vendas.Application.Interface;
using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Events.Dispatcher;
using _123Vendas.Domain.Interfaces;
using AutoMapper;

namespace _123Vendas.Application.Services;

public class ServiceVendas : IServiceVendas
{
    private readonly IRepositoryVenda _repository;
    private readonly IMapper _mapper;
    private readonly IDomainEventDispatcher _dispatcher;

    public ServiceVendas(IRepositoryVenda repository, IMapper mapper, IDomainEventDispatcher dispatcher)
    {
        _repository = repository;
        _mapper = mapper;
        _dispatcher = dispatcher;
    }

    public async Task<Guid> CriarAsync(CriarVendaDtoRequest request, CancellationToken cancellationToken = default)
    {
        var novaVenda = Venda.CriarNova(request.NumeroVenda, 
                                        request.DataEfetuacao,
                                        request.IdClienteExterno, 
                                        request.NomeCliente,
                                        request.IdFilialExterna, 
                                        request.NomeFilial);

        if (request.ItensRequest != null)
            foreach (var item in request.ItensRequest)
                novaVenda.AdicionarItem(item.IdProdutoExterno, item.ProdutoDescricao, item.Quantidade, item.ValorUnitarioProduto);

        await _repository.AdicionarAsync(novaVenda, cancellationToken);

        foreach (var evento in novaVenda.Eventos)
            await _dispatcher.PublicarEventoAsync(evento, cancellationToken);
        
        novaVenda.LimparEventos();

        return novaVenda.Id;
    }

    public async Task AtualizarAsync(AtualizarVendaDtoRequest request, CancellationToken cancellationToken = default)
    {
        var novaVenda = await _repository.ObterPorIdAsync(request.Id, cancellationToken);

        if (novaVenda is null)
        {
            throw new InvalidOperationException("Venda não encontrada.");
        }

        novaVenda.AtualizarDados(request.NomeCliente, request.NomeFilial);

        await _repository.AtualizarAsync(novaVenda, cancellationToken);

        foreach (var evento in novaVenda.Eventos)
            await _dispatcher.PublicarEventoAsync(evento, cancellationToken);

        novaVenda.LimparEventos();
    }

    public async Task CancelarAsync(Guid idVenda, CancellationToken cancellationToken = default)
    {
        var novaVenda = await _repository.ObterPorIdAsync(idVenda, cancellationToken);

        if (novaVenda is null)
        {
            throw new InvalidOperationException("Venda não encontrada.");
        }

        novaVenda.Cancelar();

        await _repository.AtualizarAsync(novaVenda, cancellationToken);

        foreach (var evento in novaVenda.Eventos)
            await _dispatcher.PublicarEventoAsync(evento, cancellationToken);

        novaVenda.LimparEventos();
    }

    public async Task AdicionarItemAsync(Guid idVenda, AdicionarItemVendaDtoRequest itemVenda, CancellationToken cancellationToken = default)
    {
        var novaVenda = await _repository.ObterPorIdAsync(idVenda, cancellationToken);

        if (novaVenda is null)
        {
            throw new InvalidOperationException("Venda não encontrada.");
        }

        novaVenda.AdicionarItem(itemVenda.IdProdutoExterno, itemVenda.ProdutoDescricao, itemVenda.Quantidade, itemVenda.ValorUnitarioProduto);

        await _repository.AtualizarAsync(novaVenda, cancellationToken);

        foreach (var evento in novaVenda.Eventos)
            await _dispatcher.PublicarEventoAsync(evento, cancellationToken);
        
        novaVenda.LimparEventos();
    }

    public async Task RemoverItemAsync(Guid idVenda, Guid idItem, CancellationToken cancellationToken = default)
    {
        var novaVenda = await _repository.ObterPorIdAsync(idVenda, cancellationToken);

        if (novaVenda is null)
        {
            throw new InvalidOperationException("Venda não encontrada.");
        }

        novaVenda.RemoverItemDaVenda(idItem);

        await _repository.AtualizarAsync(novaVenda, cancellationToken);

        foreach (var evento in novaVenda.Eventos)
            await _dispatcher.PublicarEventoAsync(evento, cancellationToken);
        
        novaVenda.LimparEventos();
    }

    public async Task<VendaDetalhesDtoResponse?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await _repository.ObterPorIdAsync(id, cancellationToken)) is { } v ? _mapper.Map<VendaDetalhesDtoResponse>(v) : null;

    public async Task<VendaDetalhesDtoResponse?> ObterPorNumeroVendaAsync(string numeroVenda, CancellationToken cancellationToken = default)
        => (await _repository.ObterPorNumeroVendaAsync(numeroVenda, cancellationToken)) is { } v ? _mapper.Map<VendaDetalhesDtoResponse>(v) : null;

    public async Task<IReadOnlyList<VendaDetalhesDtoResponse>> ListarAsync(int pagina = 1, int tamanho = 20, CancellationToken cancellationToken = default)
    {
        var vendas = await _repository.ListarAsync(pagina, tamanho, cancellationToken);
        return _mapper.Map<IReadOnlyList<VendaDetalhesDtoResponse>>(vendas);
    }
}
