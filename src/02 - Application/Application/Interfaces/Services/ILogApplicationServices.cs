using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces.Services
{
    public interface ILogApplicationServices
    {
        Task InsertLogInformacao(HttpContext context, ObjectResult objectResult);
        Task InsertLogException(HttpContext context, Exception ex);
    }
}