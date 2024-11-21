using SimpleEcommerce.Api.Domain.Catalog;

namespace SimpleEcommerce.Api.Domain.Sales
{
    public class OrderItem : Entity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Product Product { get; set; }

        public OrderItem()
        {
            
        }
        public OrderItem(int productId, string productName , int quantity , double price)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }

}
