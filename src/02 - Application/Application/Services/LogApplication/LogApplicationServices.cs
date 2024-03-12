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
        public async Task InsertLogInformacao(HttpContext context, ObjectResult objectResult)
        {
            var request = context.Request;
            string content = JsonSerializer.Serialize(objectResult.Value).ToString().Substring(0, 100);
            var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}";

            var logEntry = new LogApplication
            {
                UserName = context.User.Identity.Name,
                Date = DateTimeZoneProvider.GetBrasiliaTimeZone(DateTime.UtcNow),
                Method = request.Method,
                Path = fullUrl,
                QueryString = request.QueryString.ToString(),
                Content = content,
                TypeLog = TypeLog.Informacao.ToString(),
                StackTrace = "",
                ExceptionMessage = ""
            };

            await LogRepository.InsertAsync(logEntry);
        }

        public async Task InsertLogException(HttpContext context, Exception ex)
        {
            var request = context.Request;
            var logEntry = new LogApplication
            {
                UserName = context.User.Identity.Name,
                Date = DateTimeZoneProvider.GetBrasiliaTimeZone(DateTime.UtcNow),
                Method = request.Method,
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                Content = "",
                TypeLog = TypeLog.Exception.ToString(),
                StackTrace = ex.StackTrace,
                ExceptionMessage = $"InnerException: {ex.InnerException} ----END InnerException--- Message: {ex.Message}"
            };

            await LogRepository.InsertAsync(logEntry);
        }
    }
}
