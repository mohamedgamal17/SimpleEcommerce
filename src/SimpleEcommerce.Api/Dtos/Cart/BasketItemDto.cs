using SimpleEcommerce.Api.Dtos.Catalog;

namespace SimpleEcommerce.Api.Dtos.Cart
{
    public class BasketItemDto : EntityDto
    {
        public string ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
