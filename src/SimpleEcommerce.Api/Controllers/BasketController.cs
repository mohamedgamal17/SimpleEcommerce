using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Cart;
using SimpleEcommerce.Api.Dtos.Cart;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Models.Cart;
namespace SimpleEcommerce.Api.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    public class BasketController : Controller
    {
        private readonly IRepository<Basket> _basketRepository;
        private readonly IMapper _mapper;
        public BasketController(IRepository<Basket> basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> GetUserBasket(string userId)
        {
            var query = PrepareBasketQuery();

            var basket = await query.SingleOrDefaultAsync(x => x.UserId == userId);

            if(basket == null)
            {
                basket = new BasketDto { Id = 0, UserId = userId };
            }

            return basket;
        }


        [HttpPost("{userId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> AddOrUpdateProduct(string userId, [FromBody] BasketItemModel model)
        {
            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.AddProduct(model.ProductId, model.Quantity);

            await _basketRepository.UpdateAsync(basket);

            return await PrepareBasketQuery().SingleAsync(x => x.Id == basket.Id);
        }

        [HttpDelete("{userId}/items/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> RemoveProduct(string userId , int productId)
        {
            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.RemoveItem(productId);

            await _basketRepository.UpdateAsync(basket);

            return await PrepareBasketQuery().SingleAsync(x => x.Id == basket.Id);

        }


        [HttpPost("{userId}/clear")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> ClearBasket(string userId)
        {
            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.Clear();

            await _basketRepository.UpdateAsync(basket);

            return await PrepareBasketQuery().SingleAsync(x => x.Id == basket.Id);
        }
        private IQueryable<BasketDto> PrepareBasketQuery()
        {
           return _basketRepository.AsQuerable()
                .Include(x => x.Items)
                .ThenInclude(x=> x.Product)
                .ProjectTo<BasketDto>(_mapper.ConfigurationProvider);
        }
    }
}
