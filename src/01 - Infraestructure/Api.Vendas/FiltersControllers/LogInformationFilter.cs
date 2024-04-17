using Application.Interfaces.Services;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Vendas.FiltersControllers
{
    public class LogInformationFilter(ILogAppServices LogService) : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next
        )
        {
            if(context.Result is ObjectResult objectResult)
            {
                await LogService.RegisterLog(
                    EnumTypeLog.Information,
                    context.HttpContext,
                    objectResult
                );
            }

            await next();
        }
    }
}
