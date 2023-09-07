using Data.Repository;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Application.Configurations.Extensions
{
    public static class ApiDependenciesExtension
    {
        public static void AddApiDependencyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependecyInjectinos();
            services.AddAuthenticationJwt(configuration);
            services.AddAutoMapperConfig();
        }

        public static void AddDependecyInjectinos(this IServiceCollection services)
        {
            services.AddScoped<IVendaRepository, VendaRepository>();
        }

        public static void AddAuthenticationJwt(this IServiceCollection services, IConfiguration configuration)
        {
            //JWT
            //adiciona o manipulador de autenticacao e define o 
            //esquema de autenticacao usado : Bearer
            //valida o emissor, a audiencia e a chave
            //usando a chave secreta valida a assinatura
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidAudience = configuration["TokenConfiguration:Audience"],
                     ValidIssuer = configuration["TokenConfiguration:Issuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
                 });
        }

        public static void AddAutoMapperConfig(this IServiceCollection services)
          => services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
