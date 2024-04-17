using Api.Vendas.Controllers.Base;
using Api.Vendas.Utilities;
using Application.Utilities;
using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Venda_
{
    public class PageVendaExample : IExamplesProvider<ResponseResultDTO<PagedResult<Venda>>>
    {
        public ResponseResultDTO<PagedResult<Venda>> GetExamples()
        {
            return new ResponseResultDTO<PagedResult<Venda>>
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
                            Nome = "Teste1",
                            Preco = 1.2,
                            DataVenda = new DateTime(2023, 12, 30, 14, 07, 00),
                            QuantidadeVendido = 3,
                            TotalDaVenda = 3.59
                        },
                        new()
                        {
                            Id = 1,
                            Nome = "Teste2",
                            Preco = 1.2,
                            DataVenda = new DateTime(2023, 12, 30, 14, 07, 00),
                            QuantidadeVendido = 3,
                            TotalDaVenda = 3.59
                        }
                    ],
                },

                Mensagens = [ new Notificacao("") ]
            };
        }
    }
}
