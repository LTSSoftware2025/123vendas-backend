using _123Vendas.Domain.Events;
using _123Vendas.XUnitTest.Domain.Common;
using Shouldly;

namespace _123Vendas.XUnitTest.Domain;

public class VendaEventosTests
{
    [Fact]
    public void Deve_publicar_evento_ao_adicionar_item()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();
        novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 123", 1, 10m);

        novaVenda.Eventos.ShouldContain(e => e is ItemAdicionadoEvent);
    }

    [Fact]
    public void Deve_publicar_evento_ao_cancelar_venda()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();
        novaVenda.Cancelar();

        novaVenda.Eventos.ShouldContain(e => e is CompraCanceladaEvent);
    }

    [Fact]
    public void Deve_limpar_eventos()
    {
        var novaVenda = VendaFixture.CriarVendaDefault();
        novaVenda.AdicionarItem(Guid.NewGuid(), "Produto 456", 1, 10m);

        novaVenda.Eventos.ShouldNotBeEmpty();

        novaVenda.LimparEventos();

        novaVenda.Eventos.ShouldBeEmpty();
    }
}
