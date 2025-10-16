using _123Vendas.Domain.Entities;

namespace _123Vendas.Domain.Interfaces;

public interface IRepositoryVenda
{
    Task<Venda?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Venda?> ObterPorNumeroVendaAsync(string numeroVenda, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Venda>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Venda venda , CancellationToken cancellationToken = default);
    Task AtualizarAsync(Venda venda, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancel = default);
}
