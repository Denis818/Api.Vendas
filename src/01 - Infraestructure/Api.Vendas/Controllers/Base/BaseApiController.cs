using Application.Interfaces.Utility;
using Application.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProEventos.API.Controllers.Base
{
    public abstract class BaseApiController : Controller
    {
        protected INotificador Notificador { get; private set; }
        public BaseApiController(IServiceProvider service)
        {
            Notificador = service.GetRequiredService<INotificador>();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is not ObjectResult result)
            {
                context.Result = CustomResponse<object>(null);
                return;
            }
            context.Result = CustomResponse(result.Value);
        }

        protected IActionResult CustomResponse<TResponse>(TResponse contentResponse)
        {
            if (Notificador.ListNotificacoes.Count > 0)
            {
                var erros = Notificador.ListNotificacoes.Where(item => item.StatusCode == EnumTipoNotificacao.ClientError);
                if (erros.Any())
                {
                    var result = new ResponseResultDTO<TResponse>(default) { Mensagens = erros.ToArray() };
                    return BadRequest(result);
                }

                var errosInternos = Notificador.ListNotificacoes.Where(item => item.StatusCode == EnumTipoNotificacao.ServerError);
                if (errosInternos.Any())
                {
                    var result = new ResponseResultDTO<TResponse>(contentResponse) { Mensagens = errosInternos.ToArray() };
                    return new ObjectResult(result) { StatusCode = 500 };
                }
             
                var informacoes = Notificador.ListNotificacoes.Where(item => item.StatusCode == EnumTipoNotificacao.Informacao);
                if (informacoes.Any())
                    return Ok(new ResponseResultDTO<TResponse>(contentResponse) { Mensagens = informacoes.ToArray() });
            }

            return Ok(new ResponseResultDTO<TResponse>(contentResponse));
        }
    }

    public class ResponseResultDTO<TResponse>
    {
        public TResponse Dados { get; set; }
        public Notificacao[] Mensagens { get; set; }

        public ResponseResultDTO(TResponse data, Notificacao[] notificacoes = null)
        {
            Dados = data;
            Mensagens = notificacoes;
        }
        public ResponseResultDTO()
        {
        }
    }
}
