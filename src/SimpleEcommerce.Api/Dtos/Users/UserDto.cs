using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.Dtos.Media;

namespace SimpleEcommerce.Api.Dtos.Users
{
    public class UserDto : EntityDto<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string? AvatarId { get; set; }
        public PictureDto? Avatar { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
