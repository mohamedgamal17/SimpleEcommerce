using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.Dtos.Users;

namespace SimpleEcommerce.Api.Factories.Users
{
    public class AddressResponseFactory : ResponseFactory<Address, AddressDto>
    {
        public override Task<AddressDto> PrepareDto(Address data)
        {
            var dto = new AddressDto
            {
                Id = data.Id,
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Phone = data.Phone,
                City = data.City,
                AddressLine1 = data.AddressLine1,
                AddressLine2 = data.AddressLine2,
                Zip = data.Zip,
                UserId = data.UserId
            };

            return Task.FromResult(dto);
        }
    }
}
