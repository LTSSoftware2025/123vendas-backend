using _123Vendas.Domain.Entities;
using _123Vendas.Infra.DataModels;
using AutoMapper;

namespace _123Vendas.Infra.Context.Mappers;

public class VendaMapper : Profile
{
    public VendaMapper()
    {
        CreateMap<Venda, VendaDataModel>()
            .ForMember(vendaDataModel => vendaDataModel.ItensDaVenda, option => option.MapFrom(venda => venda.ItensDaVenda))
            .ForMember(vendaDataModel => vendaDataModel.Cancelada, option => option.MapFrom(v => v.Cancelada));

        CreateMap<ItemVenda, ItemVendaDataModel>();

        CreateMap<VendaDataModel, Venda>()
        .ConstructUsing(vendaDataModel => Venda.CriarNova(
            vendaDataModel.NumeroVenda,
            vendaDataModel.DataEfetuacao,
            vendaDataModel.IdClienteExterno,
            vendaDataModel.NomeCliente,
            vendaDataModel.IdFilialExterna,
            vendaDataModel.NomeFilial))
        .ForMember(venda => venda.ItensDaVenda, option => option.Ignore())
        .AfterMap((vendaDataModel, venda) =>
        {
            venda.LimparEventos();

            var itensField = typeof(Venda)
                .GetField("_itensDaVenda", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var itensList = (List<ItemVenda>)itensField!.GetValue(venda)!;

            foreach (var item in vendaDataModel.ItensDaVenda)
            {
                var itemVenda = ItemVenda.Criar(
                    item.IdProdutoExterno,
                    item.ProdutoDescricao,
                    item.Quantidade,
                    item.ValorUnitarioProduto
                );

                typeof(ItemVenda).GetProperty(nameof(ItemVenda.Id))!
                    .SetValue(itemVenda, item.Id);

                itensList.Add(itemVenda);
            }

            typeof(Venda).GetProperty(nameof(Venda.Id))!
                .SetValue(venda, vendaDataModel.Id);

            if (vendaDataModel.Cancelada)
            {
                typeof(Venda).GetProperty(nameof(Venda.Cancelada))!
                    .SetValue(venda, true);
            }
        });

        CreateMap<ItemVendaDataModel, ItemVenda>()
            .ConstructUsing(itemVendaDataModel => ItemVenda.Criar(itemVendaDataModel.IdProdutoExterno, itemVendaDataModel.ProdutoDescricao, itemVendaDataModel.Quantidade, itemVendaDataModel.ValorUnitarioProduto))
            .AfterMap((itemVendaDataModel, itemVenda) =>
                typeof(ItemVenda).GetProperty(nameof(ItemVenda.Id))!.SetValue(itemVenda, itemVendaDataModel.Id));
    }
}
