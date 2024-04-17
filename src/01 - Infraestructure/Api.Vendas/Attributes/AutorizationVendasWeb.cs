using Api.Vendas.Controllers.Base;
using Application.Utilities;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Api.Vendas.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizationVendasWeb : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.User.Identity.IsAuthenticated)
            {
                var response = new ResponseResultDTO<string>()
                {
                    Mensagens = [new Notificacao("Acesso não autorizado.")]
                };

                context.Result = new ObjectResult(response) { StatusCode = 401 };
                return;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PermissoesVendasWeb(params EnumPermissoes[] enumPermissoes) : Attribute, IAuthorizationFilter
    {
        private IEnumerable<string> EnumPermissoes { get; } = enumPermissoes.Select(x => x.ToString());

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var possuiTodasPermissoes = EnumPermissoes.All(permissao =>
            context.HttpContext.User.Claims.Any(claim => claim.Value == permissao));

            if(!possuiTodasPermissoes)
            {
                var response = new ResponseResultDTO<string>()
                {
                    Mensagens = [new Notificacao("Você não tem permissão para acessar esse recurso.")]
                };

                context.Result = new ObjectResult(response) { StatusCode = 401 };
                return;
            }
        }
    }
}