using Api.Vendas.Attributes;
using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Domain.Dtos.Vendas;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Controllers.Base;
using Save.Cache.Memory;

namespace Api.Vendas.Controllers.Dashboard
{
    [Cached]
    [ApiController]
    [AuthorizationVendasWeb]
    [Route("api/[controller]")]
    public class DashboardController : BaseApiController
    {
        private readonly IVendasServices _vendasServices;
        public DashboardController(IServiceProvider service, IVendasServices vendasServices)
            : base(service) => _vendasServices = vendasServices;

        [HttpGet("dia-atual")]
        public async Task<PagedResult<Venda>> GetTodaysSalesDateAsync(int paginaAtual = 1, int itensPorPagina = 10)
            => await _vendasServices.GetTodaysSalesDateAsync(paginaAtual, itensPorPagina);

        [HttpGet("vendas-por-dia")]
        public async Task<List<VendasPorDiaDto>> GetGroupSalesDayAsync()
            => await _vendasServices.GetGroupSalesDayAsync();

        [HttpGet("resumo-vendas")]
        public async Task<RemusoVendasDto> GetSalesSummaryAsync()
            => await _vendasServices.GetSalesSummaryAsync();
    }
}