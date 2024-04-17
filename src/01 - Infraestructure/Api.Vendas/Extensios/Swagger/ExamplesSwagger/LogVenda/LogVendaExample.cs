using Api.Vendas.Controllers.Base;
using Application.Utilities;
using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Log
{
    public class LogVendaExample : IExamplesProvider<ResponseResultDTO<LogVenda>>
    {
        public ResponseResultDTO<LogVenda> GetExamples()
        {
            return new ResponseResultDTO<LogVenda>
            {
                Dados = new()
                {
                    Id = 1,
                    UserName = "teste@gmail.com",
                    DataAcesso = new DateTime(2023, 12, 30, 14, 07, 00),
                    Acao = "Criação de Venda",
                    VendaId = 1335,
                    NomeProduto = "Teste",
                    PrecoProduto = 2,
                    QuantidadeVendido = 2
                },

                Mensagens = [ new Notificacao("") ]
            };
        }
    }
}
