using Application.Interfaces.Services.Usuario;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Usuario
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _acessor;
        private IEnumerable<string> _permissoes;

        public string Name => _acessor.HttpContext.User.Identity.Name;

        public UserService(IHttpContextAccessor acessor)
        {
            _acessor = acessor;
            _permissoes = acessor.HttpContext?.User?.Claims?.Select(claim => claim.Value.ToString());
        }

        public bool PossuiPermissao(params EnumPermissoes[] permissoesParaValidar)
        {
            var possuiPermissao = permissoesParaValidar
                .Select(permissao => permissao.ToString())
                .All(permissao => _permissoes.Any(x => x == permissao));

            return possuiPermissao;
        }
    }
}
