using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Exceptions;

namespace SimpleEcommerce.Api.Domain.Sales
{
    public class Order : Entity
    {
        public string UserId { get; set; }
        public double SubTotalPrice { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; private set; }

        public List<OrderItem> Items = new List<OrderItem>();
        public Order(string userId)
        {
            UserId = userId;
        }

        public void AddOrderItem(Product product , int quantity)
        {
            if (Status == OrderStatus.Pending)
            {

                var existingOrderItem = Items.SingleOrDefault(x => x.ProductId == product.Id);

                if (existingOrderItem != null)
                {
                    existingOrderItem.AddQuantity(quantity);

                }
                else
                {
                    existingOrderItem = new OrderItem(product.Id, product.Name, quantity, product.Price);

                    Items.Add(existingOrderItem);
                }

                SubTotalPrice += product.Price * quantity;

                TotalPrice += product.Price * quantity;
            }
            else
            {
                throw new BusinessLogicException($"Order can be modified only in ({OrderStatus.Pending.ToString()}) status , but it seems that current order is in status : ({Status.ToString()})");
            }
        }


    }

}
