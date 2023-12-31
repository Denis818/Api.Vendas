using Api.Vendas.Utilities;
using Application.Interfaces.Services;
using Application.Interfaces.Utility;
using Application.Utilities;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services.Usuario
{
    public class UserServices(IHttpContextAccessor acessor,
        UserManager<IdentityUser> userManager,
        INotificador notificador) : IUserServices
    {
        private readonly IHttpContextAccessor _acessor = acessor;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IEnumerable<string> _permissoes = acessor.HttpContext?.User?.Claims?.Select(claim => claim.Value.ToString());

        private readonly INotificador _notificador = notificador;

        public string Name => _acessor.HttpContext.User.Identity.Name;

        private void Notificar(string mesage, EnumTipoNotificacao tipo)
          => _notificador.Add(new Notificacao(mesage, tipo));

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
                Notificar("Usuário não encontrado.", EnumTipoNotificacao.ClientError);
                return null;
            }

            var permissonExists = EnumPermissoes.USU_000001.ToString() == permisson;
            if (!permissonExists)
            {
                Notificar("Permissão não existe.", EnumTipoNotificacao.ClientError);
                return null;
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(nameof(EnumPermissoes), permisson));
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Notificar(error.Description, EnumTipoNotificacao.ClientError);
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
                Notificar("Usuário não encontrado.", EnumTipoNotificacao.ClientError);
                return null;
            }

            var permissonExists = EnumPermissoes.USU_000001.ToString() == permisson;
            if (!permissonExists)
            {
                Notificar("Permissão não existe.", EnumTipoNotificacao.ClientError);
                return null;
            }

            var result = await _userManager.RemoveClaimAsync(user, new Claim(nameof(EnumPermissoes), permisson));
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Notificar(error.Description, EnumTipoNotificacao.ClientError);
                }
                return null;
            }

            return "Permissão removida do usuário.";
        }
    }
}
