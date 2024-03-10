using Data.DataContext;
using Data.DataContext.Context;
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
            if (!vendasDbContext.Database.CanConnect())
            {
                vendasDbContext.Database.Migrate();
                SeedVendasDbContext(vendasDbContext);
            }

            var logDbContext = serviceProvider.GetRequiredService<LogDbContext>();
            if (!logDbContext.Database.CanConnect())
            {
                logDbContext.Database.Migrate();
            }
            SeedVendasDbContext(vendasDbContext);
            PrepararUsuarioInicial(serviceScope);
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
                    var preco = Math.Round(random.NextDouble() * (10 - 1) + 1, 2); // Preço entre 1 e 10, com duas casas decim
                    var quantidade = random.Next(1, 20); // Quantidade vendida entre 1 e 20
                    var data = RandomDay(); // Data aleatória entre 01/01/2024 e 01/03/2024

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

        // Método auxiliar para gerar um DateTime aleatório entre duas datas
        private static DateTime RandomDay()
        {
            var start = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 3, 1);
            var random = new Random();
            int range = (end - start).Days;
            return start.AddDays(random.Next(range));
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
