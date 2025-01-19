using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Factories.Users;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Models.Users;
namespace SimpleEcommerce.Api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly UserResponseFactory _userResponseFactory;
        private readonly IRepository<Address> _addressRepository;
        private readonly AddressResponseFactory _addressResponseFactory;

        public UserService(IRepository<User> userRepository, UserResponseFactory userResponseFactory, IRepository<Address> addressRepository, AddressResponseFactory addressResponseFactory)
        {
            _userRepository = userRepository;
            _userResponseFactory = userResponseFactory;
            _addressRepository = addressRepository;
            _addressResponseFactory = addressResponseFactory;
        }

        public async Task<PagedDto<UserDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default)
        {
            var query = PrepareUserQuery()
                .OrderBy(x => x.Id);

            var result = await query.ToPaged(model.Skip, model.Limit);

            var response = await _userResponseFactory.PreparePagedDto(result);

            return response;
        }

        public async Task<UserDto> GetAsync(string userId, CancellationToken cancellationToken = default)
        {
            var query = PrepareUserQuery();

            var user = await query.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Domain.Users.User), userId);
            }

            var response = await _userResponseFactory.PrepareDto(user);

            return response;
        }

        public async Task<PagedDto<AddressDto>> ListAddressPagedAsync(string userId, PagingModel model, CancellationToken cancellationToken = default)
        {
            var isUserExist = await _userRepository.AnyAsync(x => x.Id == userId);

            if (!isUserExist)
            {
                throw new EntityNotFoundException(typeof(User), userId);
            }

            var result = await _addressRepository.AsQuerable()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Id)
                .ToPaged(model.Skip, model.Limit);

            var response = await _addressResponseFactory.PreparePagedDto(result);

            return response;
        }

        public async Task<AddressDto> GetAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default)
        {
            var isUserExist = await _userRepository.AnyAsync(x => x.Id == userId);

            if (!isUserExist)
            {
                throw new EntityNotFoundException(typeof(User), userId);
            }


            var result = await _addressRepository.AsQuerable()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == addressId, cancellationToken);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Address), addressId);
            }

            var resposne = await _addressResponseFactory.PrepareDto(result);

            return resposne;
        }

        public async Task<UserDto> CreateAsync(string userId, UserModel model, CancellationToken cancellationToken = default)
        {
            var isUserExist = await _userRepository.AnyAsync(x => x.Id == userId);

            if (isUserExist)
            {
                throw new BusinessLogicException("Current user already has profile");
            }

            var user = new User(userId);

            PrepareUser(user, model);

            await _userRepository.InsertAsync(user);

            var data = await PrepareUserQuery().SingleAsync(x => x.Id == user.Id);

            var response = await _userResponseFactory.PrepareDto(data);

            return response;
        }

        public async Task<UserDto> UpdateAsync(string userId, UserModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.AsQuerable()
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), userId);
            }

            PrepareUser(user, model);

            await _userRepository.UpdateAsync(user);

            var data = await PrepareUserQuery().SingleAsync(x => x.Id == user.Id);

            var response = await _userResponseFactory.PrepareDto(data);

            return response;
        }

        public async Task<AddressDto> CreateAddressAsync(string userId, AddressModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.AsQuerable()
               .Include(x => x.Addresses)
               .SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), userId);
            }

            var address = new Address
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                City = model.City,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                Phone = model.Phone,
                Zip = model.Zip,
                Email = model.Email
            };

            user.Addresses.Add(address);

            await _userRepository.UpdateAsync(user);

            var resposne = await _addressResponseFactory.PrepareDto(address);

            return resposne;
        }

        public async Task<AddressDto> UpdateAddressAsync(string userId, string addressId, AddressModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.AsQuerable()
              .Include(x => x.Addresses)
              .SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), userId);
            }

            var address = user.Addresses.SingleOrDefault(x => x.Id == addressId);

            if (address == null)
            {
                throw new EntityNotFoundException(typeof(Address), addressId);
            }

            address.FirstName = model.FirstName;
            address.LastName = model.LastName;
            address.City = model.City;
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.Phone = model.Phone;
            address.Zip = model.Zip;
            address.Email = model.Email;

            await _userRepository.UpdateAsync(user);

            var resposne = await _addressResponseFactory.PrepareDto(address);

            return resposne;
        }


        public async Task RemoveAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default)
        {
             var user = await _userRepository.AsQuerable()
              .Include(x => x.Addresses)
              .SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), userId);
            }

            var address = user.Addresses.SingleOrDefault(x => x.Id == addressId);

            if (address == null)
            {
                throw new EntityNotFoundException(typeof(Address), addressId);
            }
            user.Addresses.Remove(address);

            await _userRepository.UpdateAsync(user);
        }

        private void PrepareUser(User user, UserModel model)
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

        private IQueryable<User> PrepareUserQuery()
        {
            var query = _userRepository.AsQuerable()
                .Include(c => c.Addresses)
                .Include(c => c.Avatar);

            return query;
        }
    }
}
