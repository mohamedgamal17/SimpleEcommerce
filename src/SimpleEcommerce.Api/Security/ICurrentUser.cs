using JetBrains.Annotations;
using System.Security.Claims;

namespace SimpleEcommerce.Api.Security
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        string? Id { get; }
        string? UserName { get; }
        bool PhoneNumberVerified { get; }
        string? Email { get; }
        bool EmailVerified { get; }
        [NotNull]
        string[] Roles { get; }
        Claim? FindClaim(string claimType);
        List<Claim> FindClaims(string claimType);
        [NotNull]
        List<Claim> GetAllClaims();
        bool IsInRole(string roleName);
    }

}
