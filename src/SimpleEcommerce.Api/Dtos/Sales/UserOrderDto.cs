using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Domain.Users;

namespace SimpleEcommerce.Api.Dtos.Sales
{
    public class UserOrderDto : EntityDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string? AvatarId { get; set; }
        public Picture? Avatar { get; set; }
    }
}
