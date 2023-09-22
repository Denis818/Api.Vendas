using Data.DataContext.Context;
using Data.Repository.Base;
using Domain.Interfaces.Repository;
using Domain.Models;
using Domain.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class LogAcessoRepository : RepositoryBase<LogAcesso>, ILogAcessoRepository
    {
        public LogAcessoRepository(IServiceProvider service) : base(service)
        {
        }
    }
}