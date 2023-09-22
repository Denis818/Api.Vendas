using Api.Vendas.Attributes;
using Api.Vendas.Utilities;
using Domain.Enumeradores;
using Domain.Interfaces.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using ProEventos.API.Controllers.Base;

namespace Api.Vendas.Controllers.Log
{
    [ApiController]
    [Authorization]
    [Route("api/[controller]")]
    public class LogAcessoController : BaseApiController
    {
        private readonly ILogAcessoRepository _logAcesso;
        public LogAcessoController(IServiceProvider service, ILogAcessoRepository logAcesso) : base(service)
        {
            _logAcesso = logAcesso;
        }

        [HttpGet]
        [Permission(EnumPermissao.AcessoLog)]
        public async Task<PagedResult<LogAcesso>> Get(int paginaAtual = 1, int itensPorPagina = 10)
        {
            return await Pagination.PaginateResult(_logAcesso.Get(), paginaAtual, itensPorPagina);
        }

        [HttpGet("filter")]
        public async Task<List<LogAcesso>> FilterUserName(string name)
        {
            if (name.IsNullOrEmpty()) return await _logAcesso.Get().ToListAsync();

            var lowerName = name.ToLower();

            return await _logAcesso.Get(venda => venda.UserName.ToLower()
                                    .Contains(lowerName)).ToListAsync();
        }
    }
}
