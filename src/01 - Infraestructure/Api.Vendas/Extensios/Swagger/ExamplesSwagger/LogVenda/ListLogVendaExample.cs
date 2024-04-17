using Api.Vendas.Controllers.Base;
using Api.Vendas.Utilities;
using Application.Utilities;
using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Log
{
    public class ListLogVendaExample : IExamplesProvider<ResponseResultDTO<List<LogVenda>>>
    {
        public ResponseResultDTO<List<LogVenda>> GetExamples()
        {
            return new ResponseResultDTO<List<LogVenda>>
            {
                Dados =
                [
                    new()
                    {
                        Id = 1,
                        UserName = "teste@gmail.com",
                        DataAcesso = new DateTime(2023, 12, 30, 14, 07, 00),
                        Acao = "Criação de Venda",
                        VendaId = 1335,
                        NomeProduto = "Teste1",
                        PrecoProduto = 2,
                        QuantidadeVendido = 2
                    },
                    new()
                    {
                        Id = 2,
                        UserName = "teste@gmail.com",
                        DataAcesso = new DateTime(2023, 12, 30, 14, 07, 00),
                        Acao = "Criação de Venda",
                        VendaId = 1336,
                        NomeProduto = "Teste2",
                        PrecoProduto = 1,
                        QuantidadeVendido = 1
                    },
                    new()
                    {
                        Id = 3,
                        UserName = "teste@gmail.com",
                        DataAcesso = new DateTime(2023, 12, 30, 14, 07, 00),
                        Acao = "Deleção de Venda",
                        VendaId = 1336,
                        NomeProduto = "Teste3",
                        PrecoProduto = 1,
                        QuantidadeVendido = 1
                    }
                ],

                Mensagens = [ new Notificacao("") ]
            };
        }
    }
}
