using Api.Vendas.Utilities;
using Application.Interfaces.Services.Usuario;
using Application.Interfaces.Utility;
using Application.Utilities;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services.Usuario
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _acessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEnumerable<string> _permissoes;

        private readonly INotificador _notificador;

        public string Name => _acessor.HttpContext.User.Identity.Name;

        public UserService(IHttpContextAccessor acessor,
            UserManager<IdentityUser> userManager,
            INotificador notificador)
        {
            _acessor = acessor;
            _permissoes = acessor.HttpContext?.User?.Claims?.Select(claim => claim.Value.ToString());
            _userManager = userManager;
            _notificador = notificador;
        }

        private void Notificar(EnumTipoNotificacao tipo, string mesage)
          => _notificador.Add(new Notificacao(tipo, mesage));

        public bool PossuiPermissao(params EnumPermissoes[] permissoesParaValidar)
        {
            var possuiPermissao = permissoesParaValidar
                .Select(permissao => permissao.ToString())
                .All(permissao => _permissoes.Any(x => x == permissao));

            return possuiPermissao;
        }

        public async Task<string> AddPermissionToUser(string userEmail, string permisson)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                Notificar(EnumTipoNotificacao.ClientError, "Usuário não encontrado.");
                return null;
            }

            var permissonExists = EnumPermissoes.USU_000001.ToString() == permisson;
            if (!permissonExists)
            {
                Notificar(EnumTipoNotificacao.ClientError, "Permissão não existe.");
                return null;
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(nameof(EnumPermissoes), permisson));
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Notificar(EnumTipoNotificacao.ClientError, error.Description);
                }
                return null;
            }

            return "Permissão adicionada ao usuário.";
        }

        public async Task<string> RemovePermissionFromUser(string userEmail, string permisson)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                Notificar(EnumTipoNotificacao.ClientError, "Usuário não encontrado.");
                return null;
            }

            var permissonExists = EnumPermissoes.USU_000001.ToString() == permisson;
            if (!permissonExists)
            {
                Notificar(EnumTipoNotificacao.ClientError, "Permissão não existe.");
                return null;
            }

            var result = await _userManager.RemoveClaimAsync(user, new Claim(nameof(EnumPermissoes), permisson));
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Notificar(EnumTipoNotificacao.ClientError, error.Description);
                }
                return null;
            }

            return "Permissão removida do usuário.";
        }
    }
}
