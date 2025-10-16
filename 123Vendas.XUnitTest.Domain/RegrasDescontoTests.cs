using _123Vendas.Domain.Entities;
using Shouldly;

namespace _123Vendas.XUnitTest.Domain;

public class RegrasDescontoTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 0)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Deve_calcular_descontos_corretamente(int quantidade, decimal valorEsperado)
    {
        var resultado = RegrasDesconto.CalcularDesconto(quantidade);
        resultado.ShouldBe(valorEsperado);
    }

    [Fact]
    public void Quantidade_acima_20_produtos_iguais_deve_lancar_erro()
    {
        Should.Throw<InvalidOperationException>(() =>
            ItemVenda.Criar(Guid.NewGuid(), "Produto 123", 21, 10m));
    }
}
