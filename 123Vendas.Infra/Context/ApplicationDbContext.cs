using Microsoft.EntityFrameworkCore;

namespace _123Vendas.Infra.Context;

public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}