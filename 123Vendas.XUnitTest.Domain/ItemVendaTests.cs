using _123Vendas.Domain.Entities;
using Shouldly;

namespace _123Vendas.XUnitTest.Domain;

public class ItemVendaTests
{
    [Fact]
    public void Deve_criar_item_dados_validos()
    {
        var itemDaVenda = ItemVenda.Criar(Guid.NewGuid(), "Produto 123", 5, 10m);

        itemDaVenda.Quantidade.ShouldBe(5);
        itemDaVenda.ValorTotalItem.ShouldBe(45m);
    }

    [Fact]
    public void Deve_calcular_valor_total_desconto_20_porcento()
    {
        var itemDaVenda = ItemVenda.Criar(Guid.NewGuid(), "Produto 123", 10, 100m);

        itemDaVenda.ValorTotalItem.ShouldBe(800m);
    }

    [Fact]
    public void Nao_deve_cria_item_quantidade_negativa_ou_zerada()
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            ItemVenda.Criar(Guid.NewGuid(), "Produto 123", 0, 50m));
    }
}
