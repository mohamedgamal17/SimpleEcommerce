using Microsoft.AspNetCore.Identity;

namespace SimpleEcommerce.Api.Dtos.Identity
{
    public class IdentityUserDto : EntityDto<string>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public  bool EmailConfirmed { get; set; }
        public  bool PhoneNumberConfirmed { get; set; }
        public  bool TwoFactorEnabled { get; set; }

    }

}
