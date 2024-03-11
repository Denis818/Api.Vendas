using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Repository
{
    public interface ILogApplicationRepository
    {
        Task LogRequestAsync(HttpRequest request);
        Task LogErrorAsync(HttpRequest request, Exception exception);
        IQueryable<LogRequest> GetLogRequest();
        IQueryable<LogError> GetLogErrors();
    }
}
