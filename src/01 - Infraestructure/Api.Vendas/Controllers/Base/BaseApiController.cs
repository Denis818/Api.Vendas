using Application.Interfaces.Utility;
using Application.Utilities;
using AutoMapper;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProEventos.API.Controllers.Base
{
    public abstract class BaseApiController : Controller
    {
        private readonly INotificador _notificador;

        public BaseApiController(IServiceProvider service)
        {
            _notificador = service.GetRequiredService<INotificador>();
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
            if (_notificador.ListNotificacoes.Count > 0)
            {
                var erros = _notificador.ListNotificacoes.Where(item => item.StatusCode == EnumTipoNotificacao.ClientError);
                if (erros.Any())
                {
                    var result = new ResponseResultDTO<TResponse>(default) { Mensagens = erros.ToArray() };
                    return BadRequest(result);
                }

                var errosInternos = _notificador.ListNotificacoes.Where(item => item.StatusCode == EnumTipoNotificacao.ServerError);
                if (errosInternos.Any())
                {
                    var result = new ResponseResultDTO<TResponse>(contentResponse) { Mensagens = errosInternos.ToArray() };
                    return new ObjectResult(result) { StatusCode = 500 };
                }
             
                var informacoes = _notificador.ListNotificacoes.Where(item => item.StatusCode == EnumTipoNotificacao.Informacao);
                if (informacoes.Any())
                    return Ok(new ResponseResultDTO<TResponse>(contentResponse) { Mensagens = informacoes.ToArray() });
            }

            return Ok(new ResponseResultDTO<TResponse>(contentResponse));
        }

        protected void Notificar(EnumTipoNotificacao tipo, string mesage) 
            => _notificador.Add(new Notificacao(tipo, mesage));
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
