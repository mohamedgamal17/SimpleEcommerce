using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleEcommerce.Api.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtService(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }

        public Task<JwtToken> CreateToken(List<Claim> claims)
        {

            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = new JwtSecurityToken(
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: new SigningCredentials(

                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
                        SecurityAlgorithms.HmacSha256
                    )
                );

            var jwt = new JwtToken
            {
                Token = handler.WriteToken(jwtSecurityToken),
                NotBefore = jwtSecurityToken.ValidFrom.Ticks,
                ExpiresAt = jwtSecurityToken.ValidTo.Ticks
            };

            return Task.FromResult(jwt);
        }
    }
}
