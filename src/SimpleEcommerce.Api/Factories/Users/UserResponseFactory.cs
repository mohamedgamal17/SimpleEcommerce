using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.Factories.Media;

namespace SimpleEcommerce.Api.Factories.Users
{
    public class UserResponseFactory : ResponseFactory<User, UserDto>
    {
        private readonly MediaResponseFactory _mediaResposneFactory;
        private readonly AddressResponseFactory _addressResposneFactory;

        public UserResponseFactory(MediaResponseFactory mediaResposneFactory, AddressResponseFactory addressResposneFactory)
        {
            _mediaResposneFactory = mediaResposneFactory;
            _addressResposneFactory = addressResposneFactory;
        }

        public override async Task<UserDto> PrepareDto(User data)
        {
            var dto = new UserDto
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Gender = data.Gender,
                BirthDate = data.BirthDate,
                AvatarId = data.AvatarId
            };

            if(data.Avatar != null)
            {
                dto.Avatar = await _mediaResposneFactory.PrepareDto(data.Avatar);
            }


            if(data.Addresses != null)
            {
                dto.Addresses = await _addressResposneFactory.PrepareListDto(data.Addresses);
            }

            return dto;
        }
    }
}
