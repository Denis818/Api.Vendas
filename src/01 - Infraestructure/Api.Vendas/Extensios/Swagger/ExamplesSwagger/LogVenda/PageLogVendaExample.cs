using Api.Vendas.Controllers.Base;
using Api.Vendas.Utilities;
using Application.Utilities;
using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Log
{
    public class PageLogVendaExample : IExamplesProvider<ResponseResultDTO<PagedResult<LogVenda>>>
    {
        public ResponseResultDTO<PagedResult<LogVenda>> GetExamples()
        {
            return new ResponseResultDTO<PagedResult<LogVenda>>
            {
                Dados = new()
                {
                    TotalItens = 2,
                    PaginaAtual = 1,
                    Itens =
                    [
                        new()
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
                        new()
                        {
                            Id = 2,
                            UserName = "teste@gmail.com",
                            DataAcesso = new DateTime(2023, 12, 30, 14, 07, 00),
                            Acao = "Criação de Venda",
                            VendaId = 1336,
                            NomeProduto = "Teste",
                            PrecoProduto = 1,
                            QuantidadeVendido = 1
                        }
                    ]
                },

                Mensagens = [ new Notificacao("") ]
            };
        }
    }
}
