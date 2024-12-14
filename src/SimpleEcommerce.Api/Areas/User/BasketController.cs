using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Cart;
using SimpleEcommerce.Api.Dtos.Cart;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Models.Cart;
using System.Security.Claims;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : Controller
    {
        private readonly IRepository<Basket> _basketRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BasketController(IRepository<Basket> basketRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> GetUserBasket()
        {
            string userId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var query = PrepareBasketQuery();

            var basket = await query.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new BasketDto { Id = 0, UserId = userId };
            }

            return basket;
        }

        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> UpdateBasket(BasketModel model)
        {
            string userId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.Clear();

            if (model.Items.Count > 0)
            {
                model.Items.ForEach(item =>
                {
                    basket.AddProduct(item.ProductId, item.Quantity);
                });
            }

            await _basketRepository.UpdateAsync(basket);

            return await PrepareBasketQuery().SingleAsync(x => x.Id == basket.Id);
        }


        [HttpPost("merge")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> MergeBasket(BasketModel model)
        {
            string userId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            if(model.Items.Count > 0)
            {
                model.Items.ForEach(item =>
                {
                    basket.AddProduct(item.ProductId, item.Quantity);
                });
            }


            await _basketRepository.UpdateAsync(basket);

            return await PrepareBasketQuery().SingleAsync(x => x.Id == basket.Id);

        }

        [HttpPost("items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> AddOrUpdateProduct( [FromBody] BasketItemModel model)
        {
            string userId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.AddProduct(model.ProductId, model.Quantity);

            await _basketRepository.UpdateAsync(basket);

            return await PrepareBasketQuery().SingleAsync(x => x.Id == basket.Id);
        }

        [HttpDelete("items/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> RemoveProduct( int productId)
        {
            string userId = _httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

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
                 .ThenInclude(x => x.Product)
                 .ProjectTo<BasketDto>(_mapper.ConfigurationProvider);
        }
    }
}
