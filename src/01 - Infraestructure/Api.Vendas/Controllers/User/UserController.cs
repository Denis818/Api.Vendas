using Domain.Dtos.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Controllers.User
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> ResisterUser([FromBody] UserDto userDto)
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
                return BadRequest(userCreate.Errors);
            }

            await _signInManager.SignInAsync(user, false);

            var token = GerarToken(userDto);

            return Ok("Sucesso! seu Token: \n" + token.Token);
        }

       
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDto userDto)
        {
            var userLogin = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if (!userLogin.Succeeded)
                return BadRequest("Login Inválido....");

            return Ok(GerarToken(userDto));
        }

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
