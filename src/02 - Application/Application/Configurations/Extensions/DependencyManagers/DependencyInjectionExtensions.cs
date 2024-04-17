using Application.Interfaces.Services;
using Application.Interfaces.Utility;
using Application.Services.Usuario;
using Application.Utilities;
using Data.Repository;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using Application.Services.LogApplication;
using Application.Services.Venda_;

namespace Application.Configurations.Extensions.DependencyManagers
{
    public static class DependencyInjectionExtensions
    {
        public static void AddDependecyRepositories(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<ILogVendaRepository, LogVendaRepository>();
            services.AddScoped<ILogApplicationRepository, LogApplicationRepository>();
        }
        public static void AddDependecyServices(this IServiceCollection services)
        {
            services.AddScoped<IVendasServices, VendasServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ILogVendaServices, LogVendaServices>();
            services.AddScoped<ILogAppServices, LogAppServices>();
        }
    }
}
