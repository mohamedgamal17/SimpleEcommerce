using AutoMapper;
using SimpleEcommerce.Api.Domain.Users;

namespace SimpleEcommerce.Api.Dtos.Users
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Addresses, opt => opt.MapFrom(c => c.Addresses));

            CreateMap<Address, AddressDto>();
          
        }
    }
}
