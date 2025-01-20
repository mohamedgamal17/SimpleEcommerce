using Microsoft.AspNetCore.Identity;
using SimpleEcommerce.Api.Dtos.Identity;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Factories.Identity;
using SimpleEcommerce.Api.Models.Identity;
using SimpleEcommerce.Api.Services.Jwt;
namespace SimpleEcommerce.Api.Services.Identity
{
    public class IdentityUserService : IIdentityUserService
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IdentityUserResponseFactory _identityUserResponseFactory;

        public IdentityUserService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IJwtService jwtService, IdentityUserResponseFactory identityUserResponseFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _identityUserResponseFactory = identityUserResponseFactory;
        }

        public async Task<IdentityUserDto> CreateAsync(UserRegisterModel model, CancellationToken cancellationToken = default)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.UserName,
            };

            var identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                var errors = SerializeIdentityResult(identityResult);

                throw new ValidationException(errors);
            }

            var result =  await _userManager.FindByIdAsync(user.Id);

            var resposne = await _identityUserResponseFactory.PrepareDto(result!);

            return resposne;
        }

        public async Task<JwtToken> SignInAsync(UserLoginModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new BusinessLogicException("Invalid email or password");
            }

            var identityReuslt = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!identityReuslt.Succeeded)
            {
                var error = ExtractSignInResultError(identityReuslt);

                throw new BusinessLogicException(error);
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var claims = principal.Claims.ToList();

            var jwt = await _jwtService.CreateToken(claims);

            return jwt;
        }

        public async Task<IdentityUserDto> GetInfoAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }

            var response = await _identityUserResponseFactory.PrepareDto(user);

            return response;
        }
        public async Task ChangePasswordAsync(string userId,ChangePasswordModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }

            var identityResult = await _userManager.ChangePasswordAsync(user!, model.CurrentPassword, model.NewPassword);

            if (!identityResult.Succeeded)
            {
                var errors = SerializeIdentityResult(identityResult);

                throw new ValidationException(errors);
            }
        }

    

        public async Task<IdentityUserDto> Enable2fa(string userId, bool enabled = false, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }

            if(user.TwoFactorEnabled != enabled)
            {
                var identityResult = await _userManager.SetTwoFactorEnabledAsync(user, enabled);

                if (!identityResult.Succeeded)
                {
                    var errors = SerializeIdentityResult(identityResult);

                    throw new ValidationException(errors);
                }
            }

            var resposne = await _identityUserResponseFactory.PrepareDto(user);

            return resposne;
        }

      
        private Dictionary<string, string[]> SerializeIdentityResult(IdentityResult identityResult)
        {
            var dict = new Dictionary<string, string[]>();


            foreach (var item in identityResult.Errors)
            {
                dict.Add(item.Code, new string[] { item.Description });
            }

            return dict;
        }


        private string ExtractSignInResultError(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return "Current user is locked out";
            }

            if (result.IsNotAllowed)
            {
                return "Invalid email or password";
            }

            return string.Empty;
        }
    }
}
