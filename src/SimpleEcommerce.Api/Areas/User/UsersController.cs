using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.Models.Users;
using SimpleEcommerce.Api.Security;
using SimpleEcommerce.Api.Services.Users;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;

        public UsersController(IUserService userService, ICurrentUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<UserDto> GetCurrentUser()
        {
            string userId = _currentUser.Id!;

            var response = await _userService.GetAsync(userId);

            return response;
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<UserDto> CreateUser([FromBody] UserModel model)
        {
            string userId = _currentUser.Id!;

            var response = await _userService.CreateAsync(userId, model);

            return response;
        }

        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<UserDto> UpdateUser([FromBody] UserModel model)
        {
            string currentUserId = _currentUser.Id!;

            var response = await _userService.CreateAsync(currentUserId, model);

            return response;
        }
    }
}
