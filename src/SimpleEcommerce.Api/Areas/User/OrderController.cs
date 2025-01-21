using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Security;
using SimpleEcommerce.Api.Services.Sales;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderSerivce _orderService;
        private readonly ICurrentUser _currentUser;

        public OrderController(IOrderSerivce orderService, ICurrentUser currentUser)
        {
            _orderService = orderService;
            _currentUser = currentUser;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<OrderDto>))]
        public async Task<PagedDto<OrderDto>> GetOrdersPaged([FromQuery] PagingModel model)
        {
            string currentUserId = _currentUser.Id!;

            var response = await _orderService.ListUserOrdersPagedAsync(currentUserId, model);

            return response;
        }

        [Route("{orderId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<OrderDto> GetOrder(string orderId)
        {
            string userId = _currentUser.Id!;

            var response = await _orderService.GetUserOrderAsync(userId, orderId);

            return response;
        }
    }
}
