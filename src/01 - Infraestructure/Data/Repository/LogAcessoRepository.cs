using Data.DataContext;
using Data.DataContext.Context;
using Data.Repository.Base;
using Domain.Interfaces.Repository;
using Domain.Models;
using Domain.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class LogVendaRepository : RepositoryBase<LogVenda, LogDbContext>, ILogVendaRepository
    {
        public LogVendaRepository(IServiceProvider service) : base(service)
        {
        }
    }
}