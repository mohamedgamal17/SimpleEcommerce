using System.Security.Claims;

namespace SimpleEcommerce.Api.Services.Jwt
{
    public interface IJwtService
    {
        Task<JwtToken> CreateToken(List<Claim> claims);
    }
}
