using Api.Vendas.Attributes;
using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Domain.Enumeradores;
using Domain.Interfaces.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Controllers.Base;
using Save.Cache.Memory;

namespace Api.Vendas.Controllers.Log
{
    [ApiController]
    [AuthorizationVendasWeb]
    [Route("api/[controller]")]
    [PermissoesVendasWeb(EnumPermissoes.USU_000002)]
    public class LogController(IServiceProvider service, ILogApplicationServices logServices) : BaseApiController(service)
    {
        private readonly ILogApplicationServices _logServices = logServices;

        [HttpGet("success")]
        public async Task<PagedResult<LogRequest>> GetLogRequests(int paginaAtual = 1, int itensPorPagina = 10)
            => await _logServices.GetLogRequests(paginaAtual, itensPorPagina);

        [HttpGet("errors")]
        public async Task<PagedResult<LogError>> GetLogErrors(int paginaAtual = 1, int itensPorPagina = 10)
        {
            return await _logServices.GetLogErrors(paginaAtual, itensPorPagina);
        }
    }
}
