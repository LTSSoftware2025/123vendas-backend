using _123Vendas.XUnitTest.Domain.Common;
using Shouldly;

namespace _123Vendas.XUnitTest.Domain;

public class VendaTests
{
    [Fact]
    public void Deve_criar_venda_dados_validos()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();

        novaVenda.NumeroVenda.ShouldBe("VENDA-0001");
        novaVenda.NomeCliente.ShouldBe("Leonardo Santana");
        novaVenda.NomeFilial.ShouldBe("Filial Ambev");
        novaVenda.Cancelada.ShouldBeFalse();
    }

    [Fact]
    public void Deve_adicionar_items_e_calcular_valor_total_venda()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();

        novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 123", 3, 100m);
        novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 456", 10, 50m);

        novaVenda.ItensDaVenda.Count().ShouldBe(2);
        novaVenda.ValorTotal.ShouldBe(700m);
    }

    [Fact]
    public void Deve_remover_item_da_venda()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();
        var item = novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 123", 2, 100m);

        novaVenda.RemoverItemDaVenda(item.Id);

        novaVenda.ItensDaVenda.ShouldBeEmpty();
    }

    [Fact]
    public void Deve_cancelar_venda()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();

        novaVenda.Cancelar();

        novaVenda.Cancelada.ShouldBeTrue();
    }

    [Theory]
    [InlineData("Adicionar")]
    [InlineData("Remover")]
    public void Nao_deve_permitir_alterar_venda_cancelada(string operacao)
    {
        var novaVenda = VendaFixture.CriarVendaDefault();
        novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 123", 2, 10m);

        novaVenda.Cancelar();

        if (operacao == "Adicionar")
        {
            Should.Throw<InvalidOperationException>(() =>
                novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 456", 1, 5m));
        }
        else
        {
            var idItem = novaVenda.ItensDaVenda.First().Id;
            Should.Throw<InvalidOperationException>(() =>
                novaVenda.RemoverItemDaVenda(idItem));
        }
    }
}

