using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Models.Users;
using SimpleEcommerce.Api.Services.Users;
namespace SimpleEcommerce.Api.Areas.Admin
{
    [Route("api/[area]/users")]
    [ApiController]
    [Authorize]
    public class UsersController : AdminController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("")]
        [HttpGet]
        public async Task<PagedDto<UserDto>> GetUsersPaged([FromQuery] PagingModel model)
        {
            var response = await _userService.ListPagedAsync(model);
           
            return response;
        }

        [Route("{userId}")]
        [HttpGet]
        public async Task<UserDto> GetUser(string userId)
        {
            var response = await _userService.GetAsync(userId);

            return response;
        }

        [Route("{userId}")]
        [HttpPost]
        public async Task<UserDto> CreateUser(string userId,[FromBody] UserModel model)
        {
            var response = await _userService.CreateAsync(userId, model);

            return response;
        }

        [Route("{userId}")]
        [HttpPut]
        public async Task<UserDto> UpdateUser(string userId, [FromBody] UserModel model)
        {
            var response = await _userService.UpdateAsync(userId, model);

            return response;
        }
    }
}
