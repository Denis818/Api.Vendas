using Application.Utilities;
using Domain.Models;
using ProEventos.API.Controllers.Base;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Venda_
{
    public class VendaExample : IExamplesProvider<ResponseResultDTO<Venda>>
    {
        public ResponseResultDTO<Venda> GetExamples()
        {
            return new ResponseResultDTO<Venda>
            {
                Dados = new()
                {
                    Id = 1,
                    Nome = "Teste",
                    Preco = 1.2,
                    DataVenda = new DateTime(2023, 12, 30, 14, 07, 00),
                    QuantidadeVendido = 3,
                    TotalDaVenda = 3.59
                },

                Mensagens = [ new Notificacao("") ]
            };
        }
    }
}
