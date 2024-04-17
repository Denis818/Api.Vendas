using Api.Vendas.Controllers.Base;
using Application.Utilities;
using Domain.Dtos.Vendas;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Dashboard
{
    public class RemusoVendasDtoExample : IExamplesProvider<ResponseResultDTO<RemusoVendasDto>>
    {
        public ResponseResultDTO<RemusoVendasDto> GetExamples()
        {
            return new ResponseResultDTO<RemusoVendasDto>
            {
                Dados = new()
                {
                    MediaDeVendaPorDia = 1.2,
                    ProdutoMaisVendido = "Teste1",
                    TotalVendasHoje = 1.2,
                    TotalDeTodasAsVendas = 1.2,
                    ProdutosResumo = 
                    [
                        new() { Nome = "Teste1", TotalDaVenda = 2.50, QuantidadeTotalVendida = 1 },
                        new() { Nome = "Teste2", TotalDaVenda = 1.50, QuantidadeTotalVendida = 1 }
                    ]
                },

                Mensagens = [ new Notificacao("") ]
            };
        }
    }
}
