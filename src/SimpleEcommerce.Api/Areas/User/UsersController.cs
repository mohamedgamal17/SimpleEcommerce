using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Models.Users;
using SimpleEcommerce.Api.Security;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<Domain.Users.User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public UsersController(IRepository<Domain.Users.User> userRepository, IMapper mapper, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        [Route("")]
        [HttpGet]
        public async Task<UserDto> GetCurrentUser()
        {
            string currentUserId = _currentUser.Id!;

            var user = await _userRepository.AsQuerable()
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync(x => x.Id == currentUserId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Domain.Users.User), currentUserId);
            }

            var dto = _mapper.Map<Domain.Users.User, UserDto>(user);

            return dto;
        }

        [Route("")]
        [HttpPost]
        public async Task<UserDto> CreateUser([FromBody] UserModel model)
        {
            string currentUserId = _currentUser.Id!;

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

        [Route("")]
        [HttpPut]
        public async Task<UserDto> UpdateUser([FromBody] UserModel model)
        {
            string currentUserId = _currentUser.Id!;

            var user = await _userRepository.AsQuerable()
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync(x => x.Id == currentUserId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Domain.Users.User), currentUserId);
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
