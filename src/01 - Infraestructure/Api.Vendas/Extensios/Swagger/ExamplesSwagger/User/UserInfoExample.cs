using Application.Utilities;
using Domain.Dtos.User;
using ProEventos.API.Controllers.Base;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Vendas.Extensios.Swagger.ExamplesSwagger.User
{
    public class UserInfoExample : IExamplesProvider<ResponseResultDTO<UserInfoDto>>
    {
        public ResponseResultDTO<UserInfoDto> GetExamples()
        {
            return new ResponseResultDTO<UserInfoDto>
            {
                Dados = new()
                {
                    Email = "teste@gmail.com",
                    IsAdmin = true
                },

                Mensagens = [new Notificacao("")]
            };
        }
    }
}
