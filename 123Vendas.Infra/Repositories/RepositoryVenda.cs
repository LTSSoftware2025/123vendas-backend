using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Interfaces;
using _123Vendas.Infra.Context;
using _123Vendas.Infra.DataModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _123Vendas.Infra.Repositories;

public class RepositoryVenda : IRepositoryVenda
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RepositoryVenda(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Venda?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vendaDataModel = await _context.Vendas
            .Include(vendaDataModel => vendaDataModel.ItensDaVenda)
            .FirstOrDefaultAsync(vendaDataModel => vendaDataModel.Id == id, cancellationToken);

        return vendaDataModel is null ? null : _mapper.Map<Venda>(vendaDataModel);
    }

    public async Task<Venda?> ObterPorNumeroVendaAsync(string numeroVenda, CancellationToken cancellationToken = default)
    {
        var vendaDataModel = await _context.Vendas
            .Include(vendaDataModel => vendaDataModel.ItensDaVenda)
            .FirstOrDefaultAsync(vendaDataModel => vendaDataModel.NumeroVenda == numeroVenda, cancellationToken);

        return vendaDataModel is null ? null : _mapper.Map<Venda>(vendaDataModel);
    }

    public async Task<IReadOnlyList<Venda>> ListarAsync(int pagina = 1, int tamanho = 20, CancellationToken cancellationToken = default)
    {
        var skip = (pagina - 1) * tamanho;
        var lstVendaDataModel = await _context.Vendas
            .Include(vendaDataModel => vendaDataModel.ItensDaVenda)
            .OrderByDescending(vendaDataModel => vendaDataModel.DataEfetuacao)
            .Skip(skip).Take(tamanho)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<Venda>>(lstVendaDataModel);
    }

    public async Task AdicionarAsync(Venda venda, CancellationToken cancellationToken = default)
    {
        var vendaDataModel = _mapper.Map<VendaDataModel>(venda);

        vendaDataModel.Id = venda.Id;
        foreach (var itemVendaDataModel in vendaDataModel.ItensDaVenda)
            if (itemVendaDataModel.Id == Guid.Empty) itemVendaDataModel.Id = Guid.NewGuid();

        _context.Vendas.Add(vendaDataModel);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Venda venda, CancellationToken cancellationToken = default)
    {
        var existe = await _context.Vendas
            .Include(vendaDataModel => vendaDataModel.ItensDaVenda)
            .FirstOrDefaultAsync(vendaDataModel => vendaDataModel.Id == venda.Id, cancellationToken);

        if (existe is null)
        {
            await AdicionarAsync(venda, cancellationToken);
            return;
        }

        existe.NomeCliente = venda.NomeCliente;
        existe.NomeFilial = venda.NomeFilial;
        existe.Cancelada = venda.Cancelada;

        var itensAntigos = _context.ItensDaVenda.Where(itemVendaDataModel => itemVendaDataModel.IdVenda == existe.Id);
        _context.ItensDaVenda.RemoveRange(itensAntigos);

        foreach (var item in venda.ItensDaVenda)
        {
            var novoItem = new ItemVendaDataModel
            {
                Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id,
                IdVenda = existe.Id,
                IdProdutoExterno = item.IdProdutoExterno,
                ProdutoDescricao = item.ProdutoDescricao,
                Quantidade = item.Quantidade,
                ValorUnitarioProduto = item.ValorUnitarioProduto,
                DescontoPercentual = item.DescontoPercentual
            };

            _context.ItensDaVenda.Add(novoItem);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vendaDataModel = new VendaDataModel { Id = id };
        _context.Attach(vendaDataModel);
        _context.Remove(vendaDataModel);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
