using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos.Users;

namespace SimpleEcommerce.Api.Dtos.Sales
{
    public class OrderDto : EntityDto
    {
        public string UserId { get; set; }
        public double SubTotalPrice { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get;  set; }
        public UserDto User { get; set; }

        public List<OrderItemDto> Items = new List<OrderItemDto>();
    }
}
