using Api.Vendas.Attributes;
using Api.Vendas.Extensios.Swagger.ExamplesSwagger.User;
using Application.Interfaces.Services;
using Application.Utilities;
using Domain.Dtos.User;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProEventos.API.Controllers.Base;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        IServiceProvider service,
        IUserServices userService) : BaseApiController(service)
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserServices _userService = userService;

        [HttpPost("register")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserTokenExample))]
        public async Task<UserTokenDto> ResisterUser(UserDto userDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                Notificar("O e-mail já está registrado.", EnumTipoNotificacao.ClientError);
                return null;
            }

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
                    Notificar(errorMessage.Description, EnumTipoNotificacao.ClientError);
                }

                Notificar("Falha ao registrar.", EnumTipoNotificacao.ClientError);
                return null;
            }

            await _signInManager.SignInAsync(user, false);

            var claims = await _userManager.GetClaimsAsync(user);
            return GerarToken(userDto, claims.ToArray());
        }

        [HttpPost("login")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserTokenExample))]
        public async Task<UserTokenDto> Login(UserDto userDto)
        {
            var userLogin = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password,
                                                                     isPersistent: false, lockoutOnFailure: false);

            if (!userLogin.Succeeded)
            {
                Notificar("Email ou Senha incorretos.", EnumTipoNotificacao.ClientError);
                return null;
            }

            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var claims = await _userManager.GetClaimsAsync(user);

            return GerarToken(userDto, claims.ToArray());
        }

        [HttpGet("info")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public UserInfoDto UserInfo()
        {
            bool isAdmin = _userService.PossuiPermissao(EnumPermissoes.USU_000001);

            return new()
            {
                Email = _userService.Name,
                IsAdmin = isAdmin,
            };
        }

        [HttpPost("addPermission")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [PermissoesVendasWeb(EnumPermissoes.USU_000001)]
        public async Task<string> AddPermissionToUser(string userEmail, string permisson)
            => await _userService.AddPermissionToUser(userEmail, permisson);

        [HttpPost("removePermission")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [PermissoesVendasWeb(EnumPermissoes.USU_000001)]
        public async Task<string> RemovePermissionFromUser(string userEmail, string permisson)
            => await _userService.RemovePermissionFromUser(userEmail, permisson);

        [HttpGet("logout")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task Logout() => await _signInManager.SignOutAsync();

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

            Notificar("Data de expiração no formato UTC.", EnumTipoNotificacao.Informacao);

            return new UserTokenDto()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expirationFormat,
            };
        }
    }
}
