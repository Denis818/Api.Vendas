using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Domain.Interfaces.Repository;
using Domain.Models;

namespace Application.Services.Log
{
    public class LogApplicationServices(ILogApplicationRepository logRepository) : ILogApplicationServices
    {
        private readonly ILogApplicationRepository _logRepository = logRepository;

        public async Task<PagedResult<LogRequest>> GetLogRequests(int paginaAtual, int itensPorPagina)
            => await Pagination.PaginateResult(_logRepository.GetLogRequest(), paginaAtual, itensPorPagina);


        public async Task<PagedResult<LogError>> GetLogErrors(int paginaAtual, int itensPorPagina)
            => await Pagination.PaginateResult(_logRepository.GetLogErrors(), paginaAtual, itensPorPagina);
    }
}
