using Data.DataContext;
using Data.Repository.Base;
using Domain.Interfaces.Repository;
using Domain.Models;

namespace Data.Repository
{
    public class LogVendaRepository(IServiceProvider service) 
        : RepositoryBase<LogVenda, LogDbContext>(service), ILogVendaRepository
    {
    }
}