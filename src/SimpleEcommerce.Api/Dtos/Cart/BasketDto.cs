using SimpleEcommerce.Api.Domain;

namespace SimpleEcommerce.Api.Dtos.Cart
{
    public class BasketDto : EntityDto
    {
        public string UserId { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
