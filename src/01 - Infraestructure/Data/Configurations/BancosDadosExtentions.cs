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
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();

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
                    NormalizedUserName = "DENIS@GMAIL.COM",
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
