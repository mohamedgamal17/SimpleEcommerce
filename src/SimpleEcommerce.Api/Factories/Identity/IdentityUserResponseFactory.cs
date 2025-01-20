using Microsoft.AspNetCore.Identity;
using SimpleEcommerce.Api.Dtos.Identity;

namespace SimpleEcommerce.Api.Factories.Identity
{
    public class IdentityUserResponseFactory : ResponseFactory<IdentityUser, IdentityUserDto>
    {
        public override Task<IdentityUserDto> PrepareDto(IdentityUser data)
        {
            var dto = new IdentityUserDto
            {
                Id = data.Id,
                UserName = data.UserName,
                PhoneNumber = data.PhoneNumber,
                PhoneNumberConfirmed = data.PhoneNumberConfirmed,
                Email = data.Email,
                EmailConfirmed = data.EmailConfirmed,
                TwoFactorEnabled = data.TwoFactorEnabled
            };

            return Task.FromResult(dto);
        }
    }
}
