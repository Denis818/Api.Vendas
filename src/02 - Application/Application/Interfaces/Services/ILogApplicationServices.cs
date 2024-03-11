using Api.Vendas.Utilities;
using Domain.Models;

namespace Application.Interfaces.Services
{
    public interface ILogApplicationServices
    {
        Task<PagedResult<LogError>> GetLogErrors(int paginaAtual, int itensPorPagina);
        Task<PagedResult<LogRequest>> GetLogRequests(int paginaAtual, int itensPorPagina);
    }
}