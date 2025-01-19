using SimpleEcommerce.Api.Domain.Cart;
using SimpleEcommerce.Api.Dtos.Cart;

namespace SimpleEcommerce.Api.Factories.Cart
{
    public class BasketResposneFactory : ResponseFactory<Basket, BasketDto>
    {
        private readonly BasketItemResposneFactory _basketItemResposneFactory;

        public BasketResposneFactory(BasketItemResposneFactory basketItemResposneFactory)
        {
            _basketItemResposneFactory = basketItemResposneFactory;
        }

        public override async Task<BasketDto> PrepareDto(Basket data)
        {
            var dto = new BasketDto
            {
                Id = data.Id,
                UserId = data.UserId,
                Items = await _basketItemResposneFactory.PrepareListDto(data.Items)
            };

            return dto;
        }
    }
}
