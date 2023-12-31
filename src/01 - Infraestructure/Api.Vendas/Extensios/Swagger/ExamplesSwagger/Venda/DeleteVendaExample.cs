﻿using Application.Utilities;
using ProEventos.API.Controllers.Base;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Venda_
{
    public class DeleteVendaExample : IExamplesProvider<ResponseResultDTO<object>>
    {
        public ResponseResultDTO<object> GetExamples()
        {
            return new ResponseResultDTO<object>
            {
                Dados = null,

                Mensagens = [new Notificacao("Registro(s) Deletado(s)")]
            };
        }
    }
}
