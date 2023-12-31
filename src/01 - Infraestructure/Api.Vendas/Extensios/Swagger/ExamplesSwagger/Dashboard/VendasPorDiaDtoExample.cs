using Application.Utilities;
using Domain.Dtos.Vendas;
using ProEventos.API.Controllers.Base;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.Dashboard
{
    public class VendasPorDiaDtoExample : IExamplesProvider<ResponseResultDTO<List<VendasPorDiaDto>>>
    {
        public ResponseResultDTO<List<VendasPorDiaDto>> GetExamples()
        {
            return new ResponseResultDTO<List<VendasPorDiaDto>>
            {
                Dados =
                [
                    new() { Dia = "domingo", Total = 1482 },
                    new() { Dia = "segunda-feira", Total = 877.8 },
                    new() { Dia = "terça-feira", Total = 1035.5 },
                    new() { Dia = "quarta-feira", Total = 562.8 },
                    new() { Dia = "quinta-feira", Total = 1091.84 },
                    new() { Dia = "sexta-feira", Total = 1344.5 },
                    new() { Dia = "sábado", Total = 914.89 }
                ],

                Mensagens = [ new Notificacao("") ]
            };
        }
    }
}
