using Api.Vendas.Attributes;
using Api.Vendas.Extensios.Swagger.ExamplesSwagger.Log;
using Api.Vendas.Utilities;
using Application.Utilities;
using Domain.Enumeradores;
using Domain.Interfaces.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProEventos.API.Controllers.Base;
using Save.Cache.Memory;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Controllers.Log
{
    [Cached]
    [ApiController]
    [AuthorizationVendasWeb]
    [Route("api/[controller]")]
    [PermissoesVendasWeb(EnumPermissoes.USU_000001)]
    public class LogSalesController(IServiceProvider service, ILogVendaRepository logAcesso)
        : BaseApiController(service)
    {
        private readonly ILogVendaRepository _logAcesso = logAcesso;

        [HttpGet]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageLogVendaExample))]
        public async Task<PagedResult<LogVenda>> Get(int paginaAtual = 1, int itensPorPagina = 10)
        {
            var listLog = await Pagination.PaginateResult(_logAcesso.Get(), paginaAtual, itensPorPagina);

            if (listLog.Itens.Count == 0) 
                Notificar("Nenhum log encontrado.", EnumTipoNotificacao.Informacao);

            return listLog;
        }

        [HttpGet("{id}")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LogVendaExample))]
        public async Task<LogVenda> GetById(int id)
        {
            var logAcesso = await _logAcesso.GetByIdAsync(id);

            if (logAcesso is null)
            {
                Notificar("Nenhum log encontrado.", EnumTipoNotificacao.Informacao);
                return new LogVenda();
            }

            return logAcesso;
        }

        [HttpGet("by-email")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ListLogVendaExample))]
        public async Task<List<LogVenda>> FilterUserName(string email)
        {
            if (email.IsNullOrEmpty()) return await _logAcesso.Get().ToListAsync();

            var lowerName = email.ToLower();

            return await _logAcesso.Get(venda => venda.UserName.ToLower()
                                    .Contains(lowerName)).ToListAsync();
        }
    }
}
