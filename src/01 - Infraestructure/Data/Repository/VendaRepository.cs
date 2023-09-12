using Data.Repository.Base;
using Domain.Interfaces.Repository;
using Domain.Models;

namespace Data.Repository
{
    public class VendaRepository : RepositoryBase<Venda>, IVendaRepository
    {
        public VendaRepository(IServiceProvider service) : base(service)
        {
        }  
    }
}
