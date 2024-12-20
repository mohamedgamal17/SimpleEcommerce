﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos.Identity;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Models.Identity;
using SimpleEcommerce.Api.Services;
using System.Security.Claims;
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

        public IdentityController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IJwtService jwtService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
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
                throw new BusinessLogicException("Invalid email or password");
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
                throw new BusinessLogicException(identityResult.Errors.Select(x => x.Description));
            }

            await _userManager.FindByEmailAsync(model.Email);

            return _mapper.Map<IdentityUser, IdentityUserDto>(user);
        }


        [Route("user")]
        [HttpPost]
        [Authorize]

        public async Task<IdentityUserDto> GetUserInfo()
        {
            string currentUserId = HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(currentUserId);

            return  _mapper.Map<IdentityUser, IdentityUserDto>(user);
        }


        [Route("user/changepassword")]
        [HttpPost]
        [Authorize]
        public async Task ChangePassword([FromBody] ChangePasswordModel model)
        {
            string currentUserId = HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(currentUserId);

            var identityResult=  await _userManager.ChangePasswordAsync(user!, model.CurrentPassword, model.NewPassword);

            if (!identityResult.Succeeded)
            {
                throw new BusinessLogicException(identityResult.Errors.Select(x => x.Description));
            }
        }
    }
}
