using SimpleEcommerce.Api.Dtos.Identity;
using SimpleEcommerce.Api.Models.Identity;
using SimpleEcommerce.Api.Services.Jwt;
namespace SimpleEcommerce.Api.Services.Identity
{
    public interface IIdentityUserService  : IApplicationService
    {
        Task<JwtToken> SignInAsync(UserLoginModel model, CancellationToken cancellationToken = default);
        Task<IdentityUserDto> GetInfoAsync(string userId, CancellationToken cancellationToken = default);
        Task<IdentityUserDto> CreateAsync(UserRegisterModel model, CancellationToken cancellationToken = default);
        Task ChangePasswordAsync(string userId,ChangePasswordModel model, CancellationToken cancellationToken = default);
        Task<IdentityUserDto> Enable2fa(string userId, bool enabled = false, CancellationToken cancellationToken = default);
    }
}
