using Api.Vendas.Attributes;
using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Domain.Dtos.Vendas;
using Domain.Enumeradores;
using Domain.Models;
using Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Controllers.Base;

namespace Api.Vendas.Controllers
{
    [ApiController]
    [AuthorizationVendasWeb]
    [Route("api/[controller]")]
    public class VendaController : BaseApiController
    {
        private readonly IVendasServices _vendasServices;

        public VendaController(IServiceProvider service, IVendasServices vendasServices)
               : base(service) => _vendasServices = vendasServices;

        [HttpGet]
        public async Task<PagedResult<Venda>> GetAllVendasAsync(int paginaAtual = 1,int itensPorPagina = 10) 
            => await _vendasServices.GetAllVendasAsync(paginaAtual, itensPorPagina);    

        [HttpGet("filter")]
        public async Task<List<Venda>> FilterSalesByName(string name) 
            => await _vendasServices.FilterSalesByName(name);    

        [HttpGet("por-periodo")]
        public List<Venda> GetSalesByDate(DateTime? startDate, DateTime? endDate)
            => _vendasServices.GetSalesByDate(startDate, endDate);    

        [HttpGet("{id}")]
        public async Task<Venda> GetById(int id)
            => await _vendasServices.GetByIdAsync(id);
        
        [HttpPost]
        public async Task<Venda> Post(VendaDto vendaDto) 
            => await _vendasServices.InsertAsync(vendaDto);
        
        [HttpPut]
        public async Task<Venda> Put(int id, VendaDto vendaDto) 
            => await _vendasServices.UpdateAsync(id, vendaDto);
        
        [HttpDelete]
        public async Task Delete(int id) 
            => await _vendasServices.DeleteAsync(id);
        
        [HttpDelete("DeleteRange")]
        public async Task DeleteRanger(int[] ids) 
            => await _vendasServices.DeleteRangerAsync(ids);    
    }
}
