using Data.DataContext;
using Data.DataContext.Context;
using Domain.Converters;
using Domain.Enumeradores;
using Domain.Models;
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

          //  bool deletado = vendasDbContext.Database.EnsureDeleted();

            if (!vendasDbContext.Database.CanConnect())
            {
                vendasDbContext.Database.Migrate();
                SeedVendasDbContext(vendasDbContext);
            }

            var logDbContext = serviceProvider.GetRequiredService<LogDbContext>();
          //  bool dseletado = logDbContext.Database.EnsureDeleted();

            if (!logDbContext.Database.CanConnect())
            {
                logDbContext.Database.Migrate();
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
                        EnumPermissoes.USU_000001,
                        EnumPermissoes.USU_000002
                    };

                    var claims = listPermissoesPadroesAdmin.Select(p =>
                    new Claim(nameof(EnumPermissoes), p.ToString())).ToList();

                    _userManager.AddClaimsAsync(user, claims).Wait();
                }
            }
        }

        private static void SeedVendasDbContext(VendasDbContext context)
        {
            if (!context.Vendas.Any())
            {
                var random = new Random();
                var produtos = new List<string>
                {
                    "Bala", "Pirulito", "Chiclete", "Paçoca", "Chocolate", "Biscoito", "Goma de Mascar",
                    "Bolo", "Cupcake", "Pastilha", "Bombom", "Torrone", "Marshmallow", "Jujuba",
                    "Caramelos", "Trufa", "Brownie", "Cookie", "Muffin", "Macaron",
                    "Pão de Mel", "Brigadeiro", "Beijinho", "Cajuzinho", "Quindim",
                    "Pé de Moleque", "Cocada", "Alfajor", "Doce de Leite", "Gelatina"
                };

                for (int i = 0; i < 30; i++)
                {
                    var preco = Math.Round(random.NextDouble() * (10 - 1) + 1, 2);
                    var quantidade = random.Next(1, 20);
                    var data = RandomDay();

                    var venda = new Venda
                    {
                        Nome = produtos[random.Next(produtos.Count)],
                        Preco = preco,
                        DataVenda = data,
                        QuantidadeVendido = quantidade,
                        TotalDaVenda = Math.Round(quantidade * preco, 2)
                    };

                    context.Vendas.Add(venda);
                }

                context.SaveChanges();
            }
        }

        private static DateTime RandomDay()
        {
            var start = DateTimeZoneProvider.GetBrasiliaTimeZone(new DateTime(2024, 1, 1));
            var end = DateTimeZoneProvider.GetBrasiliaTimeZone(new DateTime(2024, 3, 1));
            var random = new Random();
            int range = (end - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
