using Data.DataContext.Context;
using Data.Repository.Base;
using Domain.Interfaces.Repository;
using Domain.Models;

namespace Data.Repository
{
    public class VendaRepository : RepositoryBase<Venda, VendasDbContext>, IVendaRepository
    {
        public VendaRepository(IServiceProvider service) : base(service)
        {
        }  
    }
}
