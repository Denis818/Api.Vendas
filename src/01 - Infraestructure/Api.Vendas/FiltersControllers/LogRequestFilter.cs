using Data.DataContext;
using Domain.Converters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Domain.Models;

namespace Api.Vendas.FiltersControllers
{
    public class LogRequestFilter(LogDbContext LogDbContext) : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var request = context.HttpContext.Request;
                string content = JsonSerializer.Serialize(objectResult.Value).ToString().Substring(0, 100);

                var logEntry = new LogRequest
                {
                    UserName = context.HttpContext.User.Identity.Name,
                    Date = DateTimeZoneProvider.GetBrasiliaTimeZone(DateTime.UtcNow),
                    Method = request.Method,
                    Path = request.Path,
                    QueryString = request.QueryString.ToString(),
                    Content = content
                };

                LogDbContext.LogRequests.Add(logEntry);
                await LogDbContext.SaveChangesAsync();
            }

            await next();
        }
    }
}
