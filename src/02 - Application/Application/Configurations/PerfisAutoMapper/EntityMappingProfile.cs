using AutoMapper;
using Domain.Dtos.Vendas;
using Domain.Models;
using Domain.Models.Dto;

namespace DadosInCached.Configurations.PerfisAutoMapper
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<Venda, VendaDto>().ReverseMap();
            CreateMap<Produto, ProdutoDto>().ReverseMap();
        }
    }
}
