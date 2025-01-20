using SimpleEcommerce.Api.Dtos.Cart;
using SimpleEcommerce.Api.Models.Cart;
namespace SimpleEcommerce.Api.Services.Cart
{
    public interface IBasketService : IApplicationService
    {
        Task<BasketDto> GetUserBasketAsync(string userId, CancellationToken cancellationToken = default);
        Task<BasketDto> UpdateBasketAsync(string userId, BasketModel basket, CancellationToken cancellationToken = default);
        Task<BasketDto> AddOrUpdateItemAsync(string userId, BasketItemModel model, CancellationToken cancellationToken = default);
        Task<BasketDto> RemoveItemAsync(string userId, string itemId, CancellationToken cancellationToken = default);
        Task<BasketDto> ClearAsync(string userId, CancellationToken cancellationToken = default);
    }
}
