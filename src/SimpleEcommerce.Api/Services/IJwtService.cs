using System.Security.Claims;

namespace SimpleEcommerce.Api.Services
{
    public interface IJwtService
    {
        Task<JwtToken> CreateToken(List<Claim> claims);
    }
}
