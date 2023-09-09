using Data.Repository.Base;
using Domain.Interfaces.Repository;
using Domain.Models;
using Domain.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class VendaRepository : RepositoryBase<Venda>, IVendaRepository
    {
        public VendaRepository(IServiceProvider service) : base(service)
        {
        }
    }
}
