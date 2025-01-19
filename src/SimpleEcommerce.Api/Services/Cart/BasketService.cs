using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Cart;
using SimpleEcommerce.Api.Dtos.Cart;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Factories.Cart;
using SimpleEcommerce.Api.Models.Cart;
namespace SimpleEcommerce.Api.Services.Cart
{
    public class BasketService : IBasketService
    {
        private readonly IRepository<Basket> _basketRepository;
        private readonly BasketResposneFactory _basketResponseFactory;

        public BasketService(IRepository<Basket> basketRepository, BasketResposneFactory basketResponseFactory)
        {
            _basketRepository = basketRepository;
            _basketResponseFactory = basketResponseFactory;
        }

        public async Task<BasketDto> GetUserBasketAsync(string userId, CancellationToken cancellationToken = default)
        {
            await CreateBasketIfNotExist(userId , cancellationToken);

            var query = PreapreBasketQuery();

            var basket = await query.SingleAsync(x => x.UserId == userId);

            var response = await _basketResponseFactory.PrepareDto(basket);

            return response;
        }

        public async Task<BasketDto> UpdateBasketAsync(string userId, BasketModel model, CancellationToken cancellationToken = default)
        {
            await CreateBasketIfNotExist(userId, cancellationToken);

            var query = PreapreBasketQuery();

            var basket = await query.SingleAsync(x => x.UserId == userId);

            basket.Clear();

            if (model.Items.Count > 0)
            {
                model.Items.ForEach(item =>
                {
                    basket.AddProduct(item.ProductId, item.Quantity);
                });
            }

            await _basketRepository.UpdateAsync(basket);

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            var response = await _basketResponseFactory.PrepareDto(result);

            return response;

        }

        public async Task<BasketDto> AddOrUpdateItemAsync(string userId, BasketItemModel model, CancellationToken cancellationToken = default)
        {
            await CreateBasketIfNotExist(userId, cancellationToken);

            var query = PreapreBasketQuery();

            var basket = await query.SingleAsync(x => x.UserId == userId);

            basket.AddProduct(model.ProductId, model.Quantity);

            await _basketRepository.UpdateAsync(basket);

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            var response = await _basketResponseFactory.PrepareDto(result);

            return response;
        }
        public async Task<BasketDto> RemoveItemAsync(string userId, string productId, CancellationToken cancellationToken = default)
        {
            await CreateBasketIfNotExist(userId);

            var basket = await _basketRepository.SingleAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.RemoveItem(productId);

            await _basketRepository.UpdateAsync(basket);

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            var response = await _basketResponseFactory.PrepareDto(result);

            return response;
        }

        public async Task<BasketDto> ClearAsync(string userId , CancellationToken cancellationToken = default)
        {
            await CreateBasketIfNotExist(userId, cancellationToken);

            var basket = await _basketRepository.SingleAsync(x => x.UserId == userId);

            basket.Clear();

            await _basketRepository.UpdateAsync(basket);

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            var response = await _basketResponseFactory.PrepareDto(result);

            return response;
        }
        private async Task CreateBasketIfNotExist(string userId, CancellationToken cancellationToken = default)
        {
            var isUserBasketExist = await _basketRepository.AnyAsync(x => x.UserId == userId);

            if (!isUserBasketExist)
            {
                var basket = new Basket() { UserId = userId };

                await _basketRepository.InsertAsync(basket);
            }
        }


        private IQueryable<Basket> PreapreBasketQuery()
        {
            return _basketRepository.AsQuerable()
                .Include(x => x.Items)
                .ThenInclude(x => x.Product);
        }
    }
}
