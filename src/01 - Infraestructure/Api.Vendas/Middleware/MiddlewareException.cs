using Application.Utilities;
using Data.DataContext;
using Domain.Interfaces.Repository;
using Domain.Models;
using ProEventos.API.Controllers.Base;
using System.Text.Json;

namespace ProEventos.API.Configuration.Middleware
{
    public class MiddlewareException(RequestDelegate next, IWebHostEnvironment environmentHost)
    {
        private readonly RequestDelegate _next = next;
        private readonly IWebHostEnvironment _environmentHost = environmentHost;

        public async Task Invoke(HttpContext httpContext)
        {
            var log = httpContext.RequestServices.GetService<ILogApplicationRepository>();

            try
            {
                await _next(httpContext);

                await log.LogRequestAsync(httpContext.Request);
            }
            catch (Exception ex)
            {
                await log.LogErrorAsync(httpContext.Request, ex);

                var message = $"Erro interno no servidor. {(_environmentHost.IsDevelopment() ? ex.Message : "")}";

                var response = new ResponseResultDTO<string>()
                {
                    Mensagens = [new(message, EnumTipoNotificacao.ServerError)]
                };

                httpContext.Response.Headers.Append("content-type", "application/json; charset=utf-8");
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        //private async Task LogRequestAsync(HttpRequest request, LogDbContext logDbContext)
        //{
        //    var logEntry = new LogRequest
        //    {
        //        Date = DateTime.UtcNow,
        //        Method = request.Method,
        //        Path = request.Path,
        //        QueryString = request.QueryString.ToString()
        //    };

        //    logDbContext.LogRequests.Add(logEntry);
        //    await logDbContext.SaveChangesAsync();
        //}

        //private async Task LogErrorAsync(HttpRequest request, Exception exception, LogDbContext logDbContext)
        //{
        //    var errorLogEntry = new LogError
        //    {
        //        Date = DateTime.UtcNow,
        //        Method = request.Method,
        //        Path = request.Path,
        //        ExceptionMessage = exception.Message,
        //        StackTrace = exception.StackTrace
        //    };

        //    logDbContext.LogErrors.Add(errorLogEntry);
        //    await logDbContext.SaveChangesAsync();
        //}
    }
}