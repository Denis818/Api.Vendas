using Application.Utilities;
using Domain.Dtos.User;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProEventos.API.Controllers.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseApiController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IServiceProvider service) : base(service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<UserTokenDto> ResisterUser(UserDto userDto)
        {
            var user = new IdentityUser
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                EmailConfirmed = true
            };

            var userCreate = await _userManager.CreateAsync(user, userDto.Password);

            if (!userCreate.Succeeded)
            {
                foreach (var errorMessage in userCreate.Errors)
                {
                    Notificar(EnumTipoNotificacao.ClientError, errorMessage.Description);
                }

                Notificar(EnumTipoNotificacao.ClientError, "Falha ao registrar.");
                return null;
            }

            await _signInManager.SignInAsync(user, false);

            var claims = await _userManager.GetClaimsAsync(user);
            return GerarToken(userDto, claims.ToArray());
        }

        [HttpPost("login")]
        public async Task<UserTokenDto> Login(UserDto userDto)
        {
            var userLogin = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password,
                                                                     isPersistent: false, lockoutOnFailure: false);

            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var claims = await _userManager.GetClaimsAsync(user);

            if (!userLogin.Succeeded)
            {
                Notificar(EnumTipoNotificacao.ClientError, "Login Inválido....");
            }

            return GerarToken(userDto, claims.ToArray());
        }

        [HttpGet("logout")]
        public async Task Logout() => await _signInManager.SignOutAsync();

        [HttpGet("info")]
        public object UserInfo()
        {
            bool isAdmin = HttpContext.User.Claims.Where(claim =>
            int.TryParse(claim.Value, out int userPermission) &&
            userPermission == (int)EnumPermissao.AcessoLog).Any();

            return new
            {
                UserEmail = HttpContext.User.Identity.Name,
                IsAdmin = isAdmin
            };
        }

        private UserTokenDto GerarToken(UserDto userDto, Claim[] permissoes)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(permissoes);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationFormat = DateTime.UtcNow.AddHours(int.Parse(_configuration["TokenConfiguration:ExpireHours"]));

            JwtSecurityToken token = new(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expirationFormat,
              signingCredentials: credenciais);

            Notificar(EnumTipoNotificacao.Informacao, "Data de expiração no formato UTC.");

            return new UserTokenDto()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expirationFormat,
                Message = "Token JWT OK"
            };
        }
    }
}
