using SimpleEcommerce.Api.Domain.Sales;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.Factories.Users;

namespace SimpleEcommerce.Api.Factories.Sales
{
    public class OrderResposneFactory : ResponseFactory<Order, OrderDto>
    {
        private readonly UserResponseFactory _userResposneFactory;
        private readonly OrderItemResposneFactory _orderItemResposneFactory;

        public OrderResposneFactory(UserResponseFactory userResposneFactory, OrderItemResposneFactory orderItemResposneFactory)
        {
            _userResposneFactory = userResposneFactory;
            _orderItemResposneFactory = orderItemResposneFactory;
        }

        public override async Task<OrderDto> PrepareDto(Order data)
        {
            var dto = new OrderDto
            {
                Id = data.Id,
                UserId = data.UserId,
                Status = data.Status,
                SubTotalPrice = data.SubTotalPrice,
                TotalPrice = data.TotalPrice,

            };

            if(data.User != null)
            {
                dto.User = await _userResposneFactory.PrepareDto(data.User);
            }

            if(data.Items != null)
            {
                dto.Items = await _orderItemResposneFactory.PrepareListDto(data.Items);
            }

            return dto;
        }
    }
}
