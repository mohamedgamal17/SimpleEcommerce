using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos.Identity;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Models.Identity;
using SimpleEcommerce.Api.Security;
using SimpleEcommerce.Api.Services;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public IdentityController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IJwtService jwtService, IMapper mapper, ICurrentUser currentUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        [Route("login")]
        [HttpPost]
        public async Task<JwtToken> Login([FromBody]UserLoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                throw new BusinessLogicException("Invalid email or password");
            }

            var identityReuslt = await  _signInManager.PasswordSignInAsync(user, model.Password, false, false);

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

        [Route("register")]
        [HttpPost]
        public async Task<IdentityUserDto> Register([FromBody] UserRegisterModel model)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.UserName,
            };

            var identityResult =  await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                var errors = SerializeIdentityResult(identityResult);

                throw new ValidationException(errors);
            }

            await _userManager.FindByEmailAsync(model.Email);

            return _mapper.Map<IdentityUser, IdentityUserDto>(user);
        }


        [Route("user")]
        [HttpPost]
        [Authorize]

        public async Task<IdentityUserDto> GetUserInfo()
        {
            string currentUserId = _currentUser.Id!;

            var user = await _userManager.FindByIdAsync(currentUserId);

            return  _mapper.Map<IdentityUser, IdentityUserDto>(user);
        }


        [Route("user/changepassword")]
        [HttpPost]
        [Authorize]
        public async Task ChangePassword([FromBody] ChangePasswordModel model)
        {
            string currentUserId = _currentUser.Id!;

            var user = await _userManager.FindByIdAsync(currentUserId);

            var identityResult=  await _userManager.ChangePasswordAsync(user!, model.CurrentPassword, model.NewPassword);

            if (!identityResult.Succeeded)
            {
                var errors = SerializeIdentityResult(identityResult);

                throw new ValidationException(errors);
            }
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


        private string ExtractSignInResultError(Microsoft.AspNetCore.Identity.SignInResult result)
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
