using _123Vendas.Domain.Entities;

namespace _123Vendas.XUnitTest.Domain.Common;

public static class VendaFixture
{
    public static Venda CriarVendaDefault()
    {
        return Venda.CriarNova(
            numeroVenda: "VENDA-0001",
            dataEfetuacao: DateTime.UtcNow,
            idClienteExterno: Guid.NewGuid(),
            nomeCliente: "Leonardo Santana",
            idFilialExterna: Guid.NewGuid(),
            nomeFilial: "Filial Ambev"
        );
    }

    public static Venda CriarVendaComItem()
    {
        var novaVenda = CriarVendaDefault();
        novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 123", 3, 50m);
        return novaVenda;
    }
}
