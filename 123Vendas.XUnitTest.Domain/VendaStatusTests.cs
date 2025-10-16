using _123Vendas.Domain.Entities;
using _123Vendas.XUnitTest.Domain.Common;
using Shouldly;

namespace _123Vendas.XUnitTest.Domain;

public class VendaStatusTests
{
    [Fact]
    public void Nova_venda_deve_comecar_nao_cancelada()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();
        
        novaVenda.Status.ShouldBe(StatusVenda.NaoCancelado);
        novaVenda.Cancelada.ShouldBeFalse();
    }

    [Fact]
    public void Ao_cancelar_venda_deve_ficar_status_cancelada()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();

        novaVenda.Cancelar();
        novaVenda.Status.ShouldBe(StatusVenda.Cancelada);
        novaVenda.Cancelada.ShouldBeTrue();
    }
}