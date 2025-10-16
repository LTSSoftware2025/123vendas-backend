using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _123Vendas.Infra.DataModels;

[Table("venda")]
public class VendaDataModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required, MaxLength(30)]
    [Column("numero_venda")]
    public string NumeroVenda { get; set; } = default!;

    [Column("data_efetuacao")]
    public DateTime DataEfetuacao { get; set; }

    [Column("id_cliente_externo")]
    public Guid IdClienteExterno { get; set; }

    [Required, MaxLength(200)]
    [Column("nome_cliente")]
    public string NomeCliente { get; set; } = default!;

    [Column("id_filial_externa")]
    public Guid IdFilialExterna { get; set; }

    [Required, MaxLength(200)]
    [Column("nome_filial")]
    public string NomeFilial { get; set; } = default!;

    [Column("cancelada")]
    public bool Cancelada { get; set; }

    public List<ItemVendaDataModel> ItensDaVenda { get; set; } = new();
}
