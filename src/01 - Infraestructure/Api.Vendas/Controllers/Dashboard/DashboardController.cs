using Api.Vendas.Attributes;
using Application.Interfaces.Services;
using Domain.Dtos.Vendas;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Controllers.Base;

namespace Api.Vendas.Controllers.Dashboard
{
    [ApiController]
    [AuthorizationVendasWeb]
    [Route("api/[controller]")]
    public class DashboardController : BaseApiController
    {
        private readonly IVendasServices _vendasServices;
        public DashboardController(IServiceProvider service, IVendasServices vendasServices)
            : base(service) => _vendasServices = vendasServices;

        [HttpGet("dia-atual")]
        public List<Venda> GetSalesCurrentDay()
            => _vendasServices.GetSalesByDate(DateTime.Now, DateTime.Now);


        [HttpGet("vendas-por-dia")]
        public async Task<List<VendasPorDiaDto>> GetGroupSalesDayAsync()
            => await _vendasServices.GetGroupSalesDayAsync();


        [HttpGet("resumo-vendas")]
        public async Task<RemusoVendasDto> GetSalesSummaryAsync()
            => await _vendasServices.GetSalesSummaryAsync();
    }
}