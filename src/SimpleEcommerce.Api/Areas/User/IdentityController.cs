using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos.Identity;
using SimpleEcommerce.Api.Models.Identity;
using SimpleEcommerce.Api.Security;
using SimpleEcommerce.Api.Services.Identity;
using SimpleEcommerce.Api.Services.Jwt;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : Controller
    {

        private readonly IIdentityUserService _identityUserService;
        private readonly ICurrentUser _currentUser;

        public IdentityController(IIdentityUserService identityUserService, ICurrentUser currentUser)
        {
            _identityUserService = identityUserService;
            _currentUser = currentUser;
        }

        [Route("login")]
        [HttpPost]
        public async Task<JwtToken> Login([FromBody]UserLoginModel model)
        {
            var jwtToken = await _identityUserService.SignInAsync(model);

            return jwtToken;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IdentityUserDto> Register([FromBody] UserRegisterModel model)
        {
            var response = await _identityUserService.CreateAsync(model);

            return response;
        }


        [Route("user")]
        [HttpPost]
        [Authorize]

        public async Task<IdentityUserDto> GetUserInfo()
        {
            string currentUserId = _currentUser.Id!;

            var user = await _identityUserService.GetInfoAsync(currentUserId);

            return user;
        }


        [Route("user/changepassword")]
        [HttpPost]
        [Authorize]
        public async Task ChangePassword([FromBody] ChangePasswordModel model)
        {
            string currentUserId = _currentUser.Id!;

            await _identityUserService.ChangePasswordAsync(currentUserId, model);
        }

    }
}
