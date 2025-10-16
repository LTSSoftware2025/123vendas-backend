namespace _123Vendas.Domain.Entities;

public static class RegrasDesconto
{
    public static decimal CalcularDesconto(int quantidade)
    {
        if (quantidade <= 4)
            return 0m;

        if (quantidade < 10)
            return 10m;

        return 20m;
    }
}
