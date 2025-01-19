using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.Factories.Catalog;
namespace SimpleEcommerce.Api.Factories.Sales
{
    public class OrderItemResposneFactory : ResponseFactory<OrderItem, OrderItemDto>
    {
        private readonly ProductResponseFactory _productResposnseFactory;

        public OrderItemResposneFactory(ProductResponseFactory productResposnseFactory)
        {
            _productResposnseFactory = productResposnseFactory;
        }

        public override async Task<OrderItemDto> PrepareDto(OrderItem data)
        {
            var dto = new OrderItemDto
            {
                Id = data.Id,
                ProductName = data.ProductName,
                ProductId = data.ProductId,
                Quantity = data.Quantity,
                Price = data.Price,

            };

            if(data.Product != null)
            {
                dto.Product = await _productResposnseFactory.PrepareDto(data.Product);
            }

            return dto;
        }
    }
}
