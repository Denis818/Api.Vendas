using Application.Utilities;
using Domain.Dtos.User;
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
                foreach (var errorMessage in userCreate .Errors)
                {
                    Notificar(EnumTipoNotificacao.ClientError, errorMessage.Description);
                }

                Notificar(EnumTipoNotificacao.ClientError, "Falha ao registrar.");
                return null;
            }

            await _signInManager.SignInAsync(user, false);

            return GerarToken(userDto);
        }
    
        [HttpPost("login")]
        public async Task<UserTokenDto> Login(UserDto userDto)
        {
            var userLogin = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if (!userLogin.Succeeded)
            {
                Notificar(EnumTipoNotificacao.ClientError, "Login Inválido....");
            }

            return GerarToken(userDto);
        }

        [HttpGet("logout")]
        public async Task Logout() => await _signInManager.SignOutAsync();
        
        [HttpGet("name-user")]
        public string NameUser() => HttpContext.User.Identity.Name;

        private UserTokenDto GerarToken(UserDto userDto)
        {
            //define declarações do usuario
            var claims = new[]
             {
                 new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Email),
                 new Claim("meuPet", "pipoca"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            //gerar uma chave com base em um algoritimo 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            //gerar a assinatura digital do token usando o algoritimo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expirationFormat = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expirationFormat,
              signingCredentials: credenciais);

            //retorna os dados com o token e informacoes
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
