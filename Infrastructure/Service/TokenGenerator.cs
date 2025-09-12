using Application.IService;
using Domain.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Service
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }
        public string GenerateToken(DTOUserResponse user)
        {
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var expired = _configuration.GetValue<int>("JwtConfig:Expired");
            var tokenExpireTimestamp = DateTime.UtcNow.AddDays(expired);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id!.ToString()),
                    new Claim(ClaimTypes.Name, user.Username!),
                    new Claim(ClaimTypes.Role, user.Role!),
                    new Claim(ClaimTypes.Email, user.Email!),
                }),
                Expires = tokenExpireTimestamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken!;
        }
    }
}
