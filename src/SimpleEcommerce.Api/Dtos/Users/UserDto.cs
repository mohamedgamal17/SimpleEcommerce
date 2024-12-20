﻿using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Domain.Users;

namespace SimpleEcommerce.Api.Dtos.Users
{
    public class UserDto : EntityDto<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int? AvatarId { get; set; }
        public Picture? Avatar { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
