using Domain.Models;

namespace Domain.Interfaces.Repository
{
    public interface ILogApplicationRepository
    {
        Task InsertAsync(LogRequest log);
        IQueryable<LogRequest> GetLogs();
    }
}
