    using _123Vendas.Application.DTOs;

    namespace _123Vendas.Application.Interface;

    public interface IServiceVendas 
    {
        Task<Guid> CriarAsync(CriarVendaDtoRequest request, CancellationToken cancellationToken = default);
        Task AtualizarAsync(AtualizarVendaDtoRequest request, CancellationToken cancellationToken = default);
        Task CancelarAsync(Guid idVenda, CancellationToken cancellationToken = default);
    
        Task AdicionarItemAsync(Guid idVenda, AdicionarItemVendaDtoRequest item, CancellationToken cancellationToken = default);
        Task RemoverItemAsync(Guid idVenda, Guid idItem, CancellationToken cancellationToken = default);

        Task<VendaDetalhesDtoResponse?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<VendaDetalhesDtoResponse?> ObterPorNumeroVendaAsync(string numeroVenda, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<VendaDetalhesDtoResponse>> ListarAsync(int pagina = 1, int tamanho = 20, CancellationToken cancellationToken = default);
    }
