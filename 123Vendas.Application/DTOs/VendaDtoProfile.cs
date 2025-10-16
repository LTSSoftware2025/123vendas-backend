using _123Vendas.Domain.Entities;
using AutoMapper;

namespace _123Vendas.Application.DTOs;

public class VendaDtoProfile : Profile
{
    public VendaDtoProfile()
    {
        CreateMap<ItemVenda, ItemVendaDtoResponse>()
            .ForMember(destino => destino.ValorTotalItem, opt => opt.MapFrom(res => res.Quantidade * res.ValorUnitarioProduto));

        CreateMap<Venda, VendaDetalhesDtoResponse>()
            .ForMember(destino => destino.Status, opt => opt.MapFrom(res => res.Status))
            .ForMember(destino => destino.ValorTotal, opt => opt.MapFrom(res => res.ValorTotal))
            .ForMember(destino => destino.ItensVendaResponse, opt => opt.MapFrom(res => res.ItensDaVenda));
    }
}
