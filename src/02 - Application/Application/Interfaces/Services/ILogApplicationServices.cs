using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces.Services
{
    public interface ILogApplicationServices
    {
        Task RegisterLog(TypeLog typeLog, HttpContext context, 
            ObjectResult objectResult = null, Exception exception = null);
    }
}