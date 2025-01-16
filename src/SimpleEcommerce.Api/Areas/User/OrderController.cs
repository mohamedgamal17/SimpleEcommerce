using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Security;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        public OrderController(IRepository<Order> orderRepository, IMapper mapper, ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        [Route("")]
        [HttpGet]
        public async Task<PagedDto<OrderDto>> GetOrdersPaged(int skip = 0, int limit = 10)
        {
            string userId = _currentUser.Id!;

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
            string userId = _currentUser.Id!;

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
