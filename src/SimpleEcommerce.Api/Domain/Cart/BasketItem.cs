using SimpleEcommerce.Api.Domain.Catalog;

namespace SimpleEcommerce.Api.Domain.Cart
{
    public class BasketItem : Entity
    {
        public string BasketId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }

        public BasketItem()
        {
            
        }

        public BasketItem(string basketId, string productId)
        {
            BasketId = basketId;
            ProductId = productId;
        }
        public BasketItem(string basketId , string productId , int quantity)
        {
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
