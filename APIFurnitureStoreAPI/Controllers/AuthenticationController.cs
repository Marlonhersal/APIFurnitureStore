using APIFurnitureStore.Share.Auth;
using APIFurnitureStoreAPI.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIFurnitureStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            IOptions<JwtConfig>  jwtConfig)
        {

            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<IActionResult> Register([FromBody] UserRegistrarionRequestDto request) {
            if (!ModelState.IsValid) return BadRequest();
            var emailExists = await _userManager.FindByEmailAsync(request.EmailAdress);
            if(emailExists != null) return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "El email ya existe"
                }
            });

            var user = new IdentityUser()
            {
                Email = request.EmailAdress,
                UserName = request.EmailAdress
            };

            var isCreated = await _userManager.CreateAsync(user);
            if(isCreated.Succeeded) {
                var token = GenerateToken(user);
                return Ok(
                    new AuthResult()
                    {
                        Result = true,
                        Token = token
                    }
                    );
            }
            else
            {
                var errors = new List<string>();
                foreach (var err in isCreated.Errors)
                    errors.Add(err.Description);

                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = errors
                });
            }
            //return BadRequest(new AuthResult
            //{
            //    Result = false,
            //    Errors = new List<string> {"User couldn´t be created" }
            //});
        }

        public string GenerateToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }
                )),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

    }
}
