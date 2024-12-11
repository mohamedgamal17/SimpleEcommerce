using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Models.Users;
using System.Security.Claims;
namespace SimpleEcommerce.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UsersController(IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [Route("")]
        [HttpGet]
        public async Task<UserDto> GetCurrentUser()
        {
            string currentUserId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _userRepository.AsQuerable()
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync(x => x.Id == currentUserId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), currentUserId);
            }

            var dto = _mapper.Map<User, UserDto>(user);

            return dto;
        }

        [Route("")]
        [HttpPost]
        public async Task<UserDto> CreateUser([FromBody]UserModel model)
        {
            string currentUserId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var isUserExist = await _userRepository.AnyAsync(x => x.Id == currentUserId);

            if (isUserExist)
            {
                throw new BusinessLogicException("Current user already has profile");
            }


            var user = new User(currentUserId);

            PrepareUser(user, model);

            await _userRepository.InsertAsync(user);

            return _mapper.Map<User, UserDto>(user);
        }

        [Route("")]
        [HttpPut]
        public async Task<UserDto> UpdateUser([FromBody] UserModel model)
        {
            string currentUserId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _userRepository.AsQuerable()
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync(x => x.Id == currentUserId);

            if(user == null)
            {
                throw new EntityNotFoundException(typeof(User), currentUserId);
            }

            PrepareUser(user, model);

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<User, UserDto>(user);
        }



        private void PrepareUser(User user , UserModel model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BirthDate = model.BirthDate;
            
            if(model.Addresses != null)
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
