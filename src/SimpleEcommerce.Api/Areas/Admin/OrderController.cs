using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Models.Sales;
namespace SimpleEcommerce.Api.Areas.Admin
{
    [Route("api/[area]/orders")]
    [ApiController]
    public class OrderController : AdminController
    {

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public OrderController(IRepository<Order> orderRepository, IRepository<Product> productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<OrderDto>))]

        public async Task<PagedDto<OrderDto>> GetOrdersPaged(int skip = 0, int limit = 10)
        {
            var query = _orderRepository
                .AsQuerable()
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            var result = await query.ToPaged(skip, limit);

            return result;

        }

        [Route("{orderId}")]
        [HttpGet]
        public async Task<OrderDto> GetOrder(string orderId)
        {
            var query = _orderRepository.AsQuerable().ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            return result;
        }

        [Route("")]
        [HttpPost]
        public async Task<OrderDto> CreateOrder([FromBody] OrderModel model)
        {
            var order = new Order(model.UserId);

            var products = await _productRepository.AsQuerable()
                .Where(x => model.Items.Any(c => c.ProductId == x.Id))
                .OrderBy(x => x.Id)
                .ToListAsync();

            var orderItems = model.Items.OrderBy(x => x.ProductId).ToList();

            foreach (var tuple in products.Zip(orderItems))
            {
                var product = tuple.First;

                var item = tuple.Second;

                if (product.Quantity < item.Quantity)
                {
                    throw new BusinessLogicException($"Product {product.Name} , existance quantity {product.Quantity}");
                }

                order.AddOrderItem(product, item.Quantity);
            }


            await _orderRepository.InsertAsync(order);

            var createdOrder = _orderRepository.AsQuerable()
                .Include(x => x.User)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .SingleAsync(x => x.Id == order.Id);


            return _mapper.Map<Order, OrderDto>(order);
        }
    }
}
