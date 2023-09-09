using AutoMapper;
using Domain.Models;
using Domain.Models.Dto;

namespace DadosInCached.Configurations.PerfisAutoMapper
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<Venda, VendaDto>().ReverseMap();
        }
    }
}
