using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Domain.Enumeradores;

namespace Api.Vendas.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int _requiredPermission;

        public PermissionAttribute(EnumPermissao permissao)
        {
            _requiredPermission = (int)permissao;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                Authorization.DenyAccess(context, StatusCodes.Status401Unauthorized, "Você não está autenticado.");
            }

            var hasPermission = context.HttpContext.User.Claims
                .Where(claim => int.TryParse(claim.Value, out int value) && value == _requiredPermission)
                .Any();

            if (!hasPermission)
            {
                Authorization.DenyAccess(context, StatusCodes.Status403Forbidden, "Acesso não autorizado.");
            }
        }  
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Authorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                DenyAccess(context, StatusCodes.Status403Forbidden, "Você não está autenticado.");
            }
        }

        public static void DenyAccess(AuthorizationFilterContext context, int statusCode, string message)
        {
            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new ObjectResult(new { Message = message })
            {
                StatusCode = statusCode
            };
        }
    }
}