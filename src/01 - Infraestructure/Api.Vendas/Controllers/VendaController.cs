using AutoMapper;
using Data.Repository;
using Domain.Dtos.Vendas;
using Domain.Interfaces.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Vendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IMapper _mapper;

        public VendaController(IVendaRepository vendaRepository, IMapper mapper, IProdutoRepository produtoRepository)
        {
            _mapper = mapper;
            _vendaRepository = vendaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _vendaRepository.Get().Include(p => p.Produtos).ToListAsync());
        }

        [HttpGet("vendas-por-dia")]
        public async Task<IActionResult> GraficoVendasPorDia()
        {
            return Ok(await _vendaRepository.GetVendasPorDia());
        }

        [HttpPost]
        public async Task<IActionResult> Post(VendaDto vendaDto)
        {
            var venda = _mapper.Map<Venda>(vendaDto);
            await _vendaRepository.InsertAsync(venda);

            if (!await _vendaRepository.SaveChangesAsync())
                return BadRequest("Ocorreu um erro ao tentar adiconar.");

            return Ok(venda);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, Venda venda)
        {
            if (id != venda.Id) return BadRequest("Ids divergentes.");

            _vendaRepository.Update(venda);

            // Salve as alterações
            if (!await _vendaRepository.SaveChangesAsync())
                return BadRequest("Ocorreu um erro ao tentar atualizar a venda.");

            return Ok(venda);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);

            if (venda == null)
                return NotFound("Não encontrado");

            _vendaRepository.Delete(venda);

            if (!await _vendaRepository.SaveChangesAsync())
                return BadRequest("Ocorreu um erro ao tentar deletar.");

            return Ok("Deletado com sucesso!");
        }
    }
}
