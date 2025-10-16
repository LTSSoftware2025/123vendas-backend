using _123Vendas.Infra.DataModels;
using Microsoft.EntityFrameworkCore;

namespace _123Vendas.Infra.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<VendaDataModel> Vendas => Set<VendaDataModel>();
    public DbSet<ItemVendaDataModel> ItensDaVenda => Set<ItemVendaDataModel>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}