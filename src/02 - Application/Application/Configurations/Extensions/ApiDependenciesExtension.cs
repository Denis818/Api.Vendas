using Application.Interfaces.Services;
using Application.Interfaces.Utility;
using Application.Services;
using Application.Utilities;
using Data.Repository;
using Domain.Interfaces.Repository;
using FluentValidation;
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
            services.AddAutoConfigs();
        }

        public static void AddDependecyInjectinos(this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<IVendasServices, VendasServices>();
            //services.AddScoped<Pagination>();
        }

        public static void AddAuthenticationJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = configuration["TokenConfiguration:Audience"],
                    ValidIssuer = configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
                });
        }

        public static void AddAutoConfigs(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
