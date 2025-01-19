using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Factories.Sales;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Models.Sales;

namespace SimpleEcommerce.Api.Services.Sales
{
    public class OrderSerivce : IOrderSerivce
    {

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly OrderResposneFactory _orderResponseFactory;

        public OrderSerivce(IRepository<Order> orderRepository, IRepository<Product> productRepository, OrderResposneFactory orderResponseFactory)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderResponseFactory = orderResponseFactory;
        }


        public async Task<PagedDto<OrderDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default)
        {
            var query = PrepareOrderQuery()
                .OrderBy(x => x.Id);

            var result = await query.ToPaged(model.Skip, model.Limit);

            var response = await _orderResponseFactory.PreparePagedDto(result);

            return response;
        }

        public async Task<OrderDto> GetAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = PrepareOrderQuery();

            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            var response = await _orderResponseFactory.PrepareDto(result);

            return response;

        }

        public async Task<PagedDto<OrderDto>> ListUserOrdersPagedAsync(string userId, PagingModel model,  CancellationToken cancellationToken = default)
        {
            var query = PrepareOrderQuery()
             .Where(x => x.UserId == userId)
             .OrderBy(x => x.Id);

            var result = await query.ToPaged(model.Skip, model.Limit);

            var response = await _orderResponseFactory.PreparePagedDto(result);

            return response;
        }

        public async Task<OrderDto> GetUserOrderAsync(string userId,string orderId , CancellationToken cancellationToken = default)
        {
            var query = PrepareOrderQuery()
              .Where(x => x.UserId == userId);

            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            var response = await _orderResponseFactory.PrepareDto(result);

            return response;
        }
        public async Task<OrderDto> CreateAsync(OrderModel model, CancellationToken cancellationToken = default)
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

            var createdOrder = await PrepareOrderQuery().SingleAsync(x => x.Id == order.Id);

            var response = await _orderResponseFactory.PrepareDto(createdOrder);

            return response;
        }
        private IQueryable<Order> PrepareOrderQuery()
        {
            var query = _orderRepository.AsQuerable()
                .Include(x => x.Items)
                .ThenInclude(c => c.Product);

            return query;
        }
    }
}
