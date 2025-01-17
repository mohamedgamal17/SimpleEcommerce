using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace SimpleEcommerce.Api.Security
{
    public class AppClaimPricnibalFactory : UserClaimsPrincipalFactory<IdentityUser>
    {
        public AppClaimPricnibalFactory(UserManager<IdentityUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {

        }
        public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var princibal = await base.CreateAsync(user);

            var identity = (ClaimsIdentity) princibal.Identity!;

            var identityUser = await UserManager.FindByIdAsync(user.Id);

            identity.AddClaim(new Claim (JwtClaimTypes.EmailVerified, identityUser!.EmailConfirmed.ToString()));

            identity.AddClaim(new Claim(JwtClaimTypes.PhoneNumberVerified, identityUser!.PhoneNumberConfirmed.ToString()));

            return princibal;
        }
    }
}
