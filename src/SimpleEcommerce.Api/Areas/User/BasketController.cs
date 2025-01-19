using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Cart;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos.Cart;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Models.Cart;
using SimpleEcommerce.Api.Security;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/basket")]
    [ApiController]
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IRepository<Basket> _basketRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ICurrentUser _currentUser;

        public BasketController(IRepository<Basket> basketRepository, IMapper mapper, IRepository<Product> productRepository, IRepository<Order> orderRepository, ICurrentUser currentUser)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> GetUserBasket()
        {
            string userId = _currentUser.Id!;

            var query = PreapreBasketQuery();

            var basket = await query.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            return _mapper.Map<Basket,BasketDto>(basket);
        }

        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> UpdateBasket(BasketModel model)
        {
            string userId = _currentUser.Id!;

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

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            return _mapper.Map<Basket, BasketDto>(result);
        }

        [HttpPost("items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> AddOrUpdateProduct( [FromBody] BasketItemModel model)
        {
            string userId = _currentUser.Id!;

            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.AddProduct(model.ProductId, model.Quantity);

            await _basketRepository.UpdateAsync(basket);

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            return _mapper.Map<Basket, BasketDto>(result);
        }

        [HttpDelete("items/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> RemoveProduct(string productId)
        {
            string userId = _currentUser.Id!;

            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.RemoveItem(productId);

            await _basketRepository.UpdateAsync(basket);

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            return _mapper.Map<Basket, BasketDto>(result);

        }


        [HttpPost("clear")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> ClearBasket()
        {
            string userId = _currentUser.Id!;

            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            basket.Clear();

            await _basketRepository.UpdateAsync(basket);

            var result = await PreapreBasketQuery().SingleAsync(x => x.Id == basket.Id);

            return _mapper.Map<Basket, BasketDto>(result);
        }

        [HttpPost("checkout")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(OrderDto))]
        public async Task<OrderDto> Checkout()
        {
            string userId = _currentUser.Id!;

            var basket = await _basketRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
            {
                basket = new Basket(userId);

                await _basketRepository.InsertAsync(basket);
            }

            if(basket.Items.Count < 1)
            {
                throw new BusinessLogicException("User basket should at least contain one item to be able to checkout");
            }

            var basketItems = basket.Items.OrderBy(x => x.ProductId).ToList();

            var products = await _productRepository.AsQuerable()
                .Where(x => basketItems.Any(c => c.ProductId == x.Id))
                .ToListAsync();


            basket.Clear();

            var order = new Order(userId);

            foreach (var tuple in products.Zip(basketItems))
            {
                var product = tuple.First;
                var basketItem = tuple.Second;

                if(product.Quantity < basketItem.Quantity)
                {
                    throw new BusinessLogicException($"Product : {product.Name} availabe quantity is {product.Quantity} , cannot place the order");
                }

                order.AddOrderItem(product, basketItem.Quantity);
            }

            await _orderRepository.InsertAsync(order);

            await _basketRepository.UpdateAsync(basket);

            var result = await _orderRepository
                .AsQuerable()
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .SingleAsync(x => x.Id == order.Id);

            return result;

        }

        private IQueryable<Basket> PreapreBasketQuery()
        {
            return _basketRepository.AsQuerable()
                .Include(x => x.Items)
                .ThenInclude(x => x.Product);           
        }
    }
}
