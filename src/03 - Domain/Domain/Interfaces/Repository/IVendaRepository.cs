using Domain.Interfaces.Repository.Base;
using Domain.Models;

namespace Domain.Interfaces.Repository
{
    public interface IVendaRepository : IRepositoryBase<Venda>
    {
        Task<List<VendaPorDia>> GetVendasPorDia();
    }
}
