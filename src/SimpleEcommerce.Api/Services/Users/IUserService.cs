using AutoMapper.QueryableExtensions;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Users;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Models.Users;
namespace SimpleEcommerce.Api.Services.Users
{
    public interface IUserService
    {
        Task<PagedDto<UserDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default);
        Task<UserDto> GetAsync(string userId, CancellationToken cancellationToken = default);
        Task<PagedDto<AddressDto>> ListAddressPagedAsync(string userId, PagingModel model, CancellationToken cancellationToken = default);
        Task<AddressDto> GetAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default);
        Task<UserDto> CreateAsync(string userId, UserModel model, CancellationToken cancellationToken = default);
        Task<UserDto> UpdateAsync(string userId, UserModel model, CancellationToken cancellationToken = default);
        Task<AddressDto> CreateAddressAsync(string userId, AddressModel model, CancellationToken cancellationToken = default);
        Task<AddressDto> UpdateAddressAsync(string userId, string addressId, AddressModel model, CancellationToken cancellationToken = default);
        Task RemoveAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default);
    }
}
