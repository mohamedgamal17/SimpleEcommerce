using System.Security.Claims;

namespace SimpleEcommerce.Api.Services.Jwt
{
    public interface IJwtService : IApplicationService
    {
        Task<JwtToken> CreateToken(List<Claim> claims);
    }
}
