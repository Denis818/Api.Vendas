using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Domain.Enumeradores;

namespace Api.Vendas.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizationVendasWeb : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PermissoesVendasWeb : Attribute, IAuthorizationFilter
    {
        private IEnumerable<string> _enumPermissoes { get; }

        public PermissoesVendasWeb(params EnumPermissoes[] enumPermissoes)
        {
            _enumPermissoes = enumPermissoes.Select(x => x.ToString());
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var possuiTodasPermissoes = _enumPermissoes.All(permissao => 
            context.HttpContext.User.Claims.Any(claim => claim.Value == permissao));

            if (!possuiTodasPermissoes)
            {
                context.Result = new ObjectResult(new { Message = "Acesso não autorizado" })
                {
                    StatusCode = 401
                };
                return;
            }
        }
    }
}