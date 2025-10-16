using _123Vendas.Domain.Entities;
using _123Vendas.Infra.DataModels;
using AutoMapper;

namespace _123Vendas.Infra.Context.Mappers;

public class VendaMapper : Profile
{
    public VendaMapper()
    {
        CreateMap<Venda, VendaDataModel>()
            .ForMember(dm => dm.ItensDaVenda, option => option.MapFrom(v => v.ItensDaVenda))
            .ForMember(dm => dm.Cancelada, option => option.MapFrom(v => v.Cancelada));

        CreateMap<ItemVenda, ItemVendaDataModel>();

        CreateMap<VendaDataModel, Venda>()
            .ConstructUsing(dm => Venda.CriarNova(
                dm.NumeroVenda,
                dm.DataEfetuacao,
                dm.IdClienteExterno,
                dm.NomeCliente,
                dm.IdFilialExterna,
                dm.NomeFilial))
            .AfterMap((dm, v) =>
            {
                foreach (var item in dm.ItensDaVenda)
                    v.AdicionarItem(item.IdProdutoExterno, item.ProdutoDescricao, item.Quantidade, item.ValorUnitarioProduto);

                    typeof(Venda).GetProperty(nameof(Venda.Id))!.SetValue(v, dm.Id);

                    if (dm.Cancelada)
                        v.Cancelar();
            });

        CreateMap<ItemVendaDataModel, ItemVenda>()
            .ConstructUsing(im => ItemVenda.Criar(im.IdProdutoExterno, im.ProdutoDescricao, im.Quantidade, im.ValorUnitarioProduto))
            .AfterMap((im, itemDom) =>
                typeof(ItemVenda).GetProperty(nameof(ItemVenda.Id))!.SetValue(itemDom, im.Id));
    }
}
