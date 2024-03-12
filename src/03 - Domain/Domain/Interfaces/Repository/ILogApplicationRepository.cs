using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Repository
{
    public interface ILogApplicationRepository
    {
        Task InsertAsync(LogApplication log);
        IQueryable<LogApplication> GetLogs();
    }
}
