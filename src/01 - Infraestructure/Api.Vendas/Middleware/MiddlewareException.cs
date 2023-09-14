using Application.Utilities;
using ProEventos.API.Controllers.Base;
using System.Text.Json;

namespace ProEventos.API.Configuration.Middleware
{
    public class MiddlewareException
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environmentHost;

        public MiddlewareException(RequestDelegate next, IWebHostEnvironment environmentHost)
        {
            _next = next;
            _environmentHost = environmentHost;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    var response = new
                    {
                        StatusCodes = httpContext.Response.StatusCode,
                        Descricao = "Voce nao esta Autenticado, ou o token esta invalido."

                    };

                    httpContext.Response.Headers.Add("content-type", "application/json; charset=utf-8");
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseResultDTO<string>();

                response.Mensagens = new Notificacao[]
                {
                    new Notificacao(
                        EnumTipoNotificacao.ServerError,
                        $"Erro interno no servidor. {(_environmentHost.IsDevelopment()? ex.Message : "")}")
                };

                httpContext.Response.Headers.Add("content-type", "application/json; charset=utf-8");
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
