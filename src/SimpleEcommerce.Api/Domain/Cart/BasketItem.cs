using SimpleEcommerce.Api.Domain.Catalog;

namespace SimpleEcommerce.Api.Domain.Cart
{
    public class BasketItem : Entity
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }

        public BasketItem()
        {
            
        }

        public BasketItem(int basketId, int productId)
        {
            BasketId = basketId;
            ProductId = productId;
        }
        public BasketItem(int basketId , int productId , int quantity)
        {
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
