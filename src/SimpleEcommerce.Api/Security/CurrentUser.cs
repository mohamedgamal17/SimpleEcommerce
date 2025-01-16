using IdentityModel;
using System.Security.Claims;

namespace SimpleEcommerce.Api.Security
{
    public class CurrentUser : ICurrentUser
    {
        public bool IsAuthenticated => _princibalAccessor.Principal != null;
        public string? Id => FindClaim(JwtClaimTypes.Subject)?.Value;
        public string? UserName => FindClaim(JwtClaimTypes.Name)?.Value;
        public string? GivenName => FindClaim(JwtClaimTypes.GivenName)?.Value;
        public string? SurName => FindClaim(JwtClaimTypes.MiddleName)?.Value;
        public string? PhoneNumber => FindClaim(JwtClaimTypes.PhoneNumber)?.Value;
        public bool PhoneNumberVerified => 
            string.Equals(FindClaim(JwtClaimTypes.PhoneNumberVerified)?.Value, "true", StringComparison.InvariantCultureIgnoreCase); 
        public string? Email => FindClaim(ClaimTypes.Email)?.Value;  
        public bool EmailVerified => string.Equals(FindClaim(JwtClaimTypes.EmailVerified)?.Value, "true", StringComparison.InvariantCultureIgnoreCase); 
        public string[] Roles => throw new NotImplementedException();

        private readonly ICurrentPrincibalAccessor _princibalAccessor;
        public CurrentUser(ICurrentPrincibalAccessor princibalAccessor)
        {
            _princibalAccessor = princibalAccessor;
        }
        public Claim? FindClaim(string claimType)
        {
            return _princibalAccessor.Principal?.Claims.SingleOrDefault(x => x.Type == claimType);
        }

        public List<Claim> FindClaims(string claimType)
        {
            return _princibalAccessor.Principal?.Claims.Where(x => x.Type == claimType).ToList() ?? new List<Claim>();
        }

        public List<Claim> GetAllClaims()
        {
            return _princibalAccessor.Principal?.Claims.ToList() ?? new List<Claim>();
        }

        public bool IsInRole(string roleName)
        {
            return FindClaims(ClaimTypes.Role).Any(x => x.Value == roleName);
        }
    }



}
