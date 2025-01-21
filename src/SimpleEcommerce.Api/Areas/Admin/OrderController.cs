using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Models.Sales;
using SimpleEcommerce.Api.Services.Sales;
namespace SimpleEcommerce.Api.Areas.Admin
{
    [Route("api/[area]/orders")]
    [ApiController]
    public class OrderController : AdminController
    {
        private readonly IOrderSerivce _orderService;

        public OrderController(IOrderSerivce orderService)
        {
            _orderService = orderService;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<OrderDto>))]

        public async Task<PagedDto<OrderDto>> GetOrdersPaged([FromQuery] PagingModel model)
        {
            var response = await _orderService.ListPagedAsync(model);

            return response;

        }

        [Route("{orderId}")]
        [HttpGet]
        public async Task<OrderDto> GetOrder(string orderId)
        {
            var response = await _orderService.GetAsync(orderId);

            return response;
        }

        [Route("")]
        [HttpPost]
        public async Task<OrderDto> CreateOrder([FromBody] OrderModel model)
        {
            var order = await _orderService.CreateAsync(model);

            return order;
        }
    }
}
