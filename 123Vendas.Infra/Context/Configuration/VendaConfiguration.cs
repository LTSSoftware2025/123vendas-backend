using _123Vendas.Infra.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _123Vendas.Infra.Context.Configuration;

public class VendaConfiguration : IEntityTypeConfiguration<VendaDataModel>
{
    public void Configure(EntityTypeBuilder<VendaDataModel> builder)
    {
        builder.ToTable("venda");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.NumeroVenda)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(v => v.DataEfetuacao)
            .IsRequired();

        builder.Property(v => v.IdClienteExterno)
            .IsRequired();

        builder.Property(v => v.NomeCliente)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(v => v.IdFilialExterna)
            .IsRequired();

        builder.Property(v => v.NomeFilial)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(v => v.Cancelada)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(v => v.ItensDaVenda)
            .WithOne(i =>  i.Venda!)
            .HasForeignKey(i => i.IdVenda)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.NumeroVenda).IsUnique(false);
        builder.HasIndex(v => v.DataEfetuacao);
        builder.HasIndex(v => v.IdClienteExterno);
        builder.HasIndex(v => v.IdFilialExterna);
    }
}
