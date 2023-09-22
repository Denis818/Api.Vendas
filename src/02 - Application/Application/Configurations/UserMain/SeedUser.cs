using Domain.Interfaces.UserMain;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Configurations.UserMain
{
    public class SeedUser : ISeedUser
    {
        private readonly UserManager<IdentityUser> _userManager;

        public SeedUser(UserManager<IdentityUser> userManager) => _userManager = userManager;

        public void CreateSeedUser()
        {
            if (_userManager.FindByEmailAsync("denis@gmail.com").Result == null)
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
                    var claims = new List<Claim>
                    {
                       new Claim("AcessoLog", "1")
                    };

                    _userManager.AddClaimsAsync(user, claims).Wait();
                }
            }
        }
    }
}
