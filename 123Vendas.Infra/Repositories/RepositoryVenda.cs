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
        var dm = await _context.Vendas
            .Include(v => v.ItensDaVenda)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        return dm is null ? null : _mapper.Map<Venda>(dm);
    }

    public async Task<Venda?> ObterPorNumeroVendaAsync(string numeroVenda, CancellationToken cancellationToken = default)
    {
        var dm = await _context.Vendas
            .Include(v => v.ItensDaVenda)
            .FirstOrDefaultAsync(v => v.NumeroVenda == numeroVenda, cancellationToken);

        return dm is null ? null : _mapper.Map<Venda>(dm);
    }

    public async Task<IReadOnlyList<Venda>> ListarAsync(int pagina = 1, int tamanho = 20, CancellationToken cancellationToken = default)
    {
        var skip = (pagina - 1) * tamanho;
        var dms = await _context.Vendas
            .Include(v => v.ItensDaVenda)
            .OrderByDescending(v => v.DataEfetuacao)
            .Skip(skip).Take(tamanho)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<Venda>>(dms);
    }

    public async Task AdicionarAsync(Venda venda, CancellationToken cancellationToken = default)
    {
        var dm = _mapper.Map<VendaDataModel>(venda);

        dm.Id = venda.Id;
        foreach (var it in dm.ItensDaVenda)
            if (it.Id == Guid.Empty) it.Id = Guid.NewGuid();

        _context.Vendas.Add(dm);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Venda venda, CancellationToken cancellationToken = default)
    {
        var existe = await _context.Vendas
            .Include(v => v.ItensDaVenda)
            .FirstOrDefaultAsync(v => v.Id == venda.Id, cancellationToken);

        if (existe is null)
        {
            await AdicionarAsync(venda, cancellationToken);
            return;
        }

        _mapper.Map(venda, existe);

        _context.ItensDaVenda.RemoveRange(existe.ItensDaVenda);
        existe.ItensDaVenda = _mapper.Map<List<ItemVendaDataModel>>(venda.ItensDaVenda);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dm = new VendaDataModel { Id = id };
        _context.Attach(dm);
        _context.Remove(dm);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
