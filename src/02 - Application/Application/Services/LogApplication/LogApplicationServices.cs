using Application.Interfaces.Services;
using Domain.Converters;
using Domain.Interfaces.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Application.Services.Logs
{
    public class LogApplicationServices(ILogApplicationRepository LogRepository) : ILogApplicationServices
    {
        public async Task RegisterLog(TypeLog typeLog, 
            HttpContext context, 
            ObjectResult objectResult, 
            Exception ex)
        {
            var request = context.Request;
            var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}";

            string content = objectResult is null ? "" : JsonSerializer.Serialize(objectResult.Value)[..100];

            var logEntry = new LogApplication
            {
                UserName = context.User.Identity.Name,
                TypeLog = typeLog.ToString(),
                Content = content,
                InclusionDate = DateTimeZoneProvider.GetBrasiliaTimeZone(DateTime.UtcNow),
                Method = request.Method,
                Path = fullUrl,
                QueryString = request.QueryString.ToString(),
                StackTrace = "",
                ExceptionMessage = ""

            };

            if (typeLog is TypeLog.Exception)
            {
                logEntry.StackTrace = ex.StackTrace;
                logEntry.ExceptionMessage = $"InnerException: {ex.InnerException} | Message: {ex.Message}";
            }

            await LogRepository.InsertAsync(logEntry);
        }
    }
}
