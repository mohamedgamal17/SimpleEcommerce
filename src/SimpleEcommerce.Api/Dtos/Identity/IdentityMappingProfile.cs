using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace SimpleEcommerce.Api.Dtos.Identity
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<IdentityUser, IdentityUserDto>();
        }
    }
}
