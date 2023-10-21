using Data.DataContext;
using Data.DataContext.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Configurations.Extensions
{
    public static class IdentityDbContextExtentions
    {
        public static void AddConectionsString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VendasDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("VENDAS")));

            services.AddDbContext<LogDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("VENDASLOG")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<VendasDbContext>()
                    .AddDefaultTokenProviders();           
        }
    }
}
