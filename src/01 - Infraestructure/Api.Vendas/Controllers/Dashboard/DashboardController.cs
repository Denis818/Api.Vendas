using Api.Vendas.Attributes;
using Api.Vendas.Extensios.Swagger.ExamplesSwagger.Dashboard;
using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Domain.Dtos.Vendas;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Controllers.Base;
using Save.Cache.Memory;
using Swashbuckle.AspNetCore.Filters;
using Api.Vendas.Extensios.Swagger.ExamplesSwagger.Venda_;

namespace Api.Vendas.Controllers.Dashboard
{
    [Cached]
    [ApiController]
    [AuthorizationVendasWeb]
    [Route("api/[controller]")]
    public class DashboardController(IServiceProvider service, IVendasServices vendasServices) : BaseApiController(service)
    {
        private readonly IVendasServices _vendasServices = vendasServices;

        [HttpGet("dia-atual")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageVendaExample))]
        public async Task<PagedResult<Venda>> GetTodaysSalesDateAsync(int paginaAtual = 1, int itensPorPagina = 10)
            => await _vendasServices.GetTodaysSalesDateAsync(paginaAtual, itensPorPagina);

        [HttpGet("vendas-por-dia")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VendasPorDiaDtoExample))]
        public async Task<List<VendasPorDiaDto>> GetGroupSalesDayAsync()
            => await _vendasServices.GetGroupSalesDayAsync();

        [HttpGet("resumo-vendas")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RemusoVendasDtoExample))]
        public async Task<RemusoVendasDto> GetSalesSummaryAsync()
            => await _vendasServices.GetSalesSummaryAsync();
    }
}