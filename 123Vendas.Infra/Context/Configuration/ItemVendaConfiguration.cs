using _123Vendas.Infra.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _123Vendas.Infra.Context.Configuration;

public class ItemVendaConfiguration : IEntityTypeConfiguration<ItemVendaDataModel>
{
    public void Configure(EntityTypeBuilder<ItemVendaDataModel> builder)
    {
        builder.ToTable("item_venda");
        builder.HasKey(iv => iv.Id);

        builder.Property(iv => iv.IdVenda)
            .IsRequired();

        builder.Property(iv => iv.IdProdutoExterno)
            .IsRequired();

        builder.Property(iv => iv.ProdutoDescricao)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(iv => iv.Quantidade)
            .IsRequired();

        builder.Property(iv => iv.ValorUnitarioProduto)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(iv => iv.DescontoPercentual)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.HasOne(iv => iv.Venda!)
            .WithMany(v => v.ItensDaVenda)
            .HasForeignKey(i => i.IdVenda)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(iv => iv.IdVenda);
    }
}
