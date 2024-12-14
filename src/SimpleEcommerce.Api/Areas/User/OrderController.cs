using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using System.Security.Claims;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;
        public OrderController(IRepository<Order> orderRepository,  IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [Route("")]
        [HttpGet]
        public async Task<PagedDto<OrderDto>> GetOrdersPaged(int skip = 0, int limit = 10)
        {
            string userId = HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var query = _orderRepository
                .AsQuerable()
                .Where(x=> x.UserId == userId)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            var result = await query.ToPaged(skip, limit);

            return result;

        }

        [Route("{orderId}")]
        [HttpGet]
        public async Task<OrderDto> GetOrder(int orderId)
        {
            string userId = HttpContext!.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var query = _orderRepository
                .AsQuerable()
                .Where(x => x.UserId == userId)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            return result;
        }
    }
}
