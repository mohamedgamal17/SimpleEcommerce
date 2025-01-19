using SimpleEcommerce.Api.Domain.Cart;
using SimpleEcommerce.Api.Dtos.Cart;
using SimpleEcommerce.Api.Factories.Catalog;

namespace SimpleEcommerce.Api.Factories.Cart
{
    public class BasketItemResposneFactory : ResponseFactory<BasketItem, BasketItemDto>
    {
        private readonly ProductResponseFactory _productResposnseFactory;

        public BasketItemResposneFactory(ProductResponseFactory productResposnseFactory)
        {
            _productResposnseFactory = productResposnseFactory;
        }

        public override async Task<BasketItemDto> PrepareDto(BasketItem data)
        {
            var dto = new BasketItemDto
            {
                Id = data.Id,
                ProductId = data.ProductId,
                Quantity = data.Quantity,
                Product = await _productResposnseFactory.PrepareDto(data.Product)
            };

            return dto;
        }
    }
}
