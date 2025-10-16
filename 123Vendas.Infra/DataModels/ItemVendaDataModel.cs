using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _123Vendas.Infra.DataModels;

[Table("item_venda")]
public class ItemVendaDataModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("id_venda")]
    public Guid IdVenda { get; set; }

    [Column("id_produto_externo")]
    public Guid IdProdutoExterno { get; set; }

    [Required, MaxLength(200)]
    [Column("produto_descricao")]
    public string ProdutoDescricao { get; set; } = default!;

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Column("valor_unitario_produto", TypeName = "decimal(18,2)")]
    public decimal ValorUnitarioProduto { get; set; }

    [Column("desconto_percentual", TypeName = "decimal(5,2)")]
    public decimal DescontoPercentual { get; set; }

    public VendaDataModel? Venda { get; set; }
}
