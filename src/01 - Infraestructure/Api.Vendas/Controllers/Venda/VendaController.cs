using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Domain.Dtos.Vendas;
using Domain.Models;
using Domain.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Controllers.Base;

namespace Api.Vendas.Controllers
{
//    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VendaController : BaseApiController
    {
        private readonly IVendasServices _vendasServices;

        public VendaController(IServiceProvider service,
            IVendasServices vendasServices) : base(service)
        {
            _vendasServices = vendasServices;
        }

        [HttpGet]
        public async Task<PagedResult<Venda>> GetAllVendasAsync(int paginaAtual = 1,
            int itensPorPagina = 10)
        {
            return await _vendasServices.GetAllVendasAsync(paginaAtual, itensPorPagina);
        }

        [HttpGet("dia-atual")]
        public List<Venda> GetSalesCurrentDay()
        {
            return _vendasServices.GetSalesByDate(DateTime.Now, DateTime.Now);
        }

        [HttpGet("por-periodo")]
        public List<Venda> GetSalesByDate(DateTime? startDate, DateTime? endDate)
        {
            return _vendasServices.GetSalesByDate(startDate, endDate);
        }

        [HttpGet("filter")]
        public async Task<List<Venda>> FilterSalesByName(string name)
        {
            return await _vendasServices.FilterSalesByName(name);
        }

        [HttpGet("vendas-por-dia")]
        public async Task<List<VendasPorDiaDto>> GetGroupSalesDayAsync()
        {
            return await _vendasServices.GetGroupSalesDayAsync();
        }

        [HttpGet("resumo-vendas")]
        public async Task<RemusoVendasDto> GetSalesSummaryAsync()
        {
           return await _vendasServices.GetSalesSummaryAsync();
        }

        [HttpGet("{id}")]
        public async Task<Venda> GetById(int id)
        {
            return await _vendasServices.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<Venda> Post(VendaDto vendaDto)
        {
            return await _vendasServices.InsertAsync(vendaDto);
        }

        [HttpPut]
        public async Task<Venda> Put(int id, VendaDto vendaDto)
        {
            return await _vendasServices.UpdateAsync(id, vendaDto);
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await _vendasServices.DeleteAsync(id);
        }

        [HttpDelete("DeleteRange")]
        public async Task DeleteRanger(int[] ids)
        {
            await _vendasServices.DeleteRangerAsync(ids);
        }
    }
}
