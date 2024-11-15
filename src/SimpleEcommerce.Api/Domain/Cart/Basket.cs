using SimpleEcommerce.Api.Exceptions;

namespace SimpleEcommerce.Api.Domain.Cart
{
    public class Basket : Entity
    {
        public string UserId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public Basket()
        {
            
        }

        public Basket(string userId)
        {
            UserId = userId;
        }


        public void AddProduct(int productId , int quantity)
        {
            var item = Items.SingleOrDefault(x => x.ProductId == productId);

            if(item == null)
            {
                item = new BasketItem(Id, quantity);

                Items.Add(item);
            }

            item.Quantity += quantity;

            if(item.Quantity <= 0)
            {
                Items.Remove(item);
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.SingleOrDefault(x => x.ProductId == productId);
            
            if(item == null)
            {
                throw new EntityNotFoundException(typeof(BasketItem), productId);
            }

            Items.Remove(item);
        }

        public void Migrate(Basket basket)
        {
            basket.Items.ForEach(item =>
            {
                AddProduct(item.ProductId, item.Quantity);
            });
        }

    }
}
