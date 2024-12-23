using Microsoft.IdentityModel.Tokens;
using ServiceAuth.Domain.Entities;
using ServiceAuth.WebApi.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceAuth.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        public TokenService(JwtConfig jwtCongin)
        {
            _jwtConfig = jwtCongin ?? throw new ArgumentNullException(nameof(jwtCongin));
        }
        public string GenerateToken(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = CreateClaimsIdentity(account),
                Expires = DateTime.UtcNow.Add(_jwtConfig.LifeTime),
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_jwtConfig.SigningKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity CreateClaimsIdentity(Account account)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString(), account.Email.Value)
        });
            return claimsIdentity;
        }
    }
}