﻿using Application.Utilities;
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
            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                var message = $"Erro interno no servidor. {(_environmentHost.IsDevelopment() ? ex.Message : "")}";

                var response = new ResponseResultDTO<string>()
                {
                    Mensagens = [ new(message, EnumTipoNotificacao.ServerError) ]
                };

                httpContext.Response.Headers.Append("content-type", "application/json; charset=utf-8");
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
