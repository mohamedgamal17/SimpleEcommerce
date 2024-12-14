using SimpleEcommerce.Api.Dtos.Catalog;

namespace SimpleEcommerce.Api.Dtos.Sales
{
    public class OrderItemDto : EntityDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public ProductDto Product { get; set; }
    }
}
