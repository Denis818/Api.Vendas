using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;

namespace Api.Vendas.FiltersControllers
{
    public class LogInformationFilter(ILogApplicationServices LogService) : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                await LogService.InsertLogInformacao(context.HttpContext, objectResult);
            }

            await next();
        }   
    }
}
