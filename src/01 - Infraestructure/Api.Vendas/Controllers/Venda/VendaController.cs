using Api.Vendas.Attributes;
using Api.Vendas.Extensios.Swagger.ExamplesSwagger.Venda_;
using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Domain.Models;
using Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Controllers.Base;
using Save.Cache.Memory;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Controllers
{
    [Cached]
    [ApiController]
    [AuthorizationVendasWeb]
    [Route("api/[controller]")]
    public class VendaController(IServiceProvider service, IVendasServices vendasServices) : BaseApiController(service)
    {
        private readonly IVendasServices _vendasServices = vendasServices;

        [HttpGet]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageVendaExample))]
        public async Task<PagedResult<Venda>> GetAllVendasAsync(int paginaAtual = 1, int itensPorPagina = 10)
            => await _vendasServices.GetAllVendasAsync(paginaAtual, itensPorPagina);

        [HttpGet("filterByName")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ListVendaExample))]
        public async Task<List<Venda>> FilterSalesByName(string name)
            => await _vendasServices.FilterSalesByName(name);

        [HttpGet("por-periodo")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ListVendaExample))]
        public List<Venda> GetSalesByDate(DateTime? startDate, DateTime? endDate)
            => _vendasServices.GetSalesByDate(startDate, endDate);

        [HttpGet("{id}")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VendaExample))]
        public async Task<Venda> GetById(int id)
            => await _vendasServices.GetByIdAsync(id);

        [HttpPost]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VendaExample))]
        public async Task<Venda> Post(VendaDto vendaDto)
            => await _vendasServices.InsertAsync(vendaDto);

        [HttpPut]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VendaExample))]
        public async Task<Venda> Put(int id, VendaDto vendaDto)
            => await _vendasServices.UpdateAsync(id, vendaDto);

        [HttpDelete]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteVendaExample))]
        public async Task Delete(int id)
            => await _vendasServices.DeleteAsync(id);

        [HttpDelete("DeleteRange")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteVendaExample))]
        public async Task DeleteRanger(int[] ids)
            => await _vendasServices.DeleteRangerAsync(ids);
    }
}
