using _123Vendas.Domain.Entities;
using Shouldly;

namespace _123Vendas.XUnitTest.Domain;

public class VendaCalculoTests
{
    [Fact]
    public void Deve_calcular_valor_total_considerando_descontos()
    {
        var venda = Venda.CriarNova("VENDA-0001", DateTime.UtcNow, Guid.NewGuid(), "Leonardo Santana", Guid.NewGuid(), "Filial Ambev");

        venda.AdicionarItem(Guid.NewGuid(), "Produto Especial Ambev", 3, 100m);

        venda.AdicionarItem(Guid.NewGuid(), "Produto Simples Ambev", 10, 50m);

        venda.ValorTotal.ShouldBe(700m);
    }
}
