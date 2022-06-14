using Application.DTO;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IRepository repository, IConfiguration configuration, ILogger<LoginController> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger; ;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            
            try
            {
                if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                    return BadRequest("Username and/or Password not specified");

                var user = await _repository.SingleAsync<UserEntity>(f1=> f1.UserName == loginDTO.UserName && f1.Password == loginDTO.Password);
                if (user != null)
                {
                    var role = await _repository.SingleAsync<RoleEntity>(f1 => f1.Id == user.RoleId);
                    var token = GetToken(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, loginDTO.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, role.Code),
                    });
                    _logger.LogInformation($"user: {user.UserName} - logginig");
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
