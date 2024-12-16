using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Models.Users;
using System.Security.Claims;
namespace SimpleEcommerce.Api.Areas.Admin
{
    [Route("api/[area]/users")]
    [ApiController]
    [Authorize]
    public class UsersController : AdminController
    {
        private readonly IRepository<Domain.Users.User> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UsersController(IRepository<Domain.Users.User> userRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [Route("")]
        [HttpGet]
        public async Task<PagedDto<UserDto>> GetUsersPaged(int skip, int limit = 10)
        {
            var query = _userRepository.AsQuerable()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider);


            var result = await query.ToPaged(skip, limit);

            return result;
        }

        [Route("{userId}")]
        [HttpGet]
        public async Task<UserDto> GetUser(string userId)
        {
            var query = _userRepository.AsQuerable()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.Id == userId);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Domain.Users.User), userId);
            }

            return result;
        }

        [Route("")]
        [HttpPost]
        public async Task<UserDto> CreateUser([FromBody] UserModel model)
        {
            string currentUserId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var isUserExist = await _userRepository.AnyAsync(x => x.Id == currentUserId);

            if (isUserExist)
            {
                throw new BusinessLogicException("Current user already has profile");
            }


            var user = new Domain.Users.User(currentUserId);

            PrepareUser(user, model);

            await _userRepository.InsertAsync(user);

            return _mapper.Map<Domain.Users.User, UserDto>(user);
        }

        [Route("{userId}")]
        [HttpPut]
        public async Task<UserDto> UpdateUser(string userId, [FromBody] UserModel model)
        {

            var user = await _userRepository.AsQuerable()
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Domain.Users.User), userId);
            }

            PrepareUser(user, model);

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<Domain.Users.User, UserDto>(user);
        }



        private void PrepareUser(Domain.Users.User user, UserModel model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BirthDate = model.BirthDate;
            user.AvatarId = model.AvatarId;
            if (model.Addresses != null)
            {
                user.Addresses = model.Addresses.Select(x => new Address
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    City = x.City,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    Phone = x.Phone,
                    Zip = x.Zip,
                    Email = x.Email
                }).ToList();
            }
        }
    }
}
