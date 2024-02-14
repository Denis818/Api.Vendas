using Data.DataContext;
using Data.DataContext.Context;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Application.Configurations.UserMain
{
    public static class SeedUser
    {
        public static void ConfigurarBancoDados(this IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var vendasDbContext = serviceProvider.GetRequiredService<VendasDbContext>();
            if (!vendasDbContext.Database.CanConnect()) // Verifica se é possível conectar ao banco de dados Vendas
            {
                vendasDbContext.Database.Migrate(); // Aplica as migrações se o banco de dados não existir
            }

            var logDbContext = serviceProvider.GetRequiredService<LogDbContext>();
            if (!logDbContext.Database.CanConnect()) // Verifica se é possível conectar ao banco de dados Log
            {
                logDbContext.Database.Migrate(); // Aplica as migrações se o banco de dados não existir
            }

            PrepararUsuarioInicial(serviceScope);
        }


        public static void PrepararUsuarioInicial(IServiceScope serviceScope)
        {
            var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var usuarioInicial = _userManager.FindByEmailAsync("denis@gmail.com").GetAwaiter().GetResult();

            if (usuarioInicial is null)
            {
                IdentityUser user = new()
                {
                    UserName = "denis@gmail.com",
                    Email = "denis@gmail.com",
                    NormalizedUserName = "denis@gmail.com",
                    NormalizedEmail = "DENIS@GMAIL.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = _userManager.CreateAsync(user, "Denis@123456").Result;

                if (result.Succeeded)
                {
                    var listPermissoesPadroesAdmin = new[]
                    {
                        EnumPermissoes.USU_000001
                    };

                    var claims = listPermissoesPadroesAdmin.Select(p => 
                    new Claim(nameof(EnumPermissoes), p.ToString())).ToList();

                    _userManager.AddClaimsAsync(user, claims).Wait();
                }
            }
        }
    }
}
