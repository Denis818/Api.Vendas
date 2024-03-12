using Data.DataContext;
using Domain.Converters;
using Domain.Interfaces.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Data.Repository
{
    public class LogApplicationRepository(LogDbContext logDbContext) : ILogApplicationRepository
    {
        private readonly LogDbContext _logDbContext = logDbContext;

        public async Task LogErrorAsync(HttpRequest request, Exception exception)
        {
            var errorLogEntry = new LogError
            {
                Date = DateTimeZoneProvider.GetBrasiliaTimeZone(DateTime.UtcNow),
                Method = request.Method,
                Path = request.Path,
                ExceptionMessage = exception.Message,
                StackTrace = exception.StackTrace,
            };

            _logDbContext.LogErrors.Add(errorLogEntry);
            await _logDbContext.SaveChangesAsync();
        }

        public IQueryable<LogRequest> GetLogRequest()
        {
            return _logDbContext.LogRequests;
        }

        public IQueryable<LogError> GetLogErrors()
        {
            return _logDbContext.LogErrors;
        }
    }
}
