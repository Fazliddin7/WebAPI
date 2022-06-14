using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace WebAPI.Services
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("Expiration")]
        public DateTime Expiration { get; set; }
    }

    public interface IJwtAuthManager
    {
        JwtAuthResult GenerateTokens(IEnumerable<Claim> claims);
    }
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _configuration;
        public JwtAuthManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtAuthResult GenerateTokens(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtAuthResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
    }
}
