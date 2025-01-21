using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos.Cart;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Models.Cart;
using SimpleEcommerce.Api.Models.Sales;
using SimpleEcommerce.Api.Security;
using SimpleEcommerce.Api.Services.Cart;
using SimpleEcommerce.Api.Services.Sales;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/basket")]
    [ApiController]
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderSerivce _orderService;
        private readonly ICurrentUser _currentUser;

        public BasketController(IBasketService basketService, IOrderSerivce orderService, ICurrentUser currentUser)
        {
            _basketService = basketService;
            _orderService = orderService;
            _currentUser = currentUser;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> GetUserBasket()
        {
            string userId = _currentUser.Id!;

            var response = await _basketService.GetUserBasketAsync(userId);

            return response;
        }

        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> UpdateBasket(BasketModel model)
        {
            string userId = _currentUser.Id!;

            var response = await _basketService.UpdateBasketAsync(userId, model);

            return response;
        }

        [HttpPost("items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> AddOrUpdateProduct([FromBody] BasketItemModel model)
        {
            string userId = _currentUser.Id!;

            var response = await _basketService.AddOrUpdateItemAsync(userId, model);

            return response;
        }

        [HttpDelete("items/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> RemoveProduct(string productId)
        {
            string userId = _currentUser.Id!;

            var response = await _basketService.RemoveItemAsync(userId, productId);

            return response;

        }


        [HttpPost("clear")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<BasketDto> ClearBasket()
        {
            string userId = _currentUser.Id!;

            var response = await _basketService.ClearAsync(userId);

            return response;
        }

        [HttpPost("checkout")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<OrderDto> Checkout()
        {
            string userId = _currentUser.Id!;

            var basket = await _basketService.GetUserBasketAsync(userId);

            if (basket.Items.Count < 1)
            {
                throw new BusinessLogicException("User basket should at least contain one item to be able to checkout");
            }

            var orderModel = new OrderModel
            {
                UserId = userId,
                Items = basket.Items.Select(x => new OrderItemModel { ProductId = x.ProductId, Quantity = x.Quantity }).ToList()
            };

            var order = await _orderService.CreateAsync(orderModel);

            await _basketService.ClearAsync(userId);

            return order;

        }
    }
}
