using AutoMapper;
using SimpleEcommerce.Api.Domain.Sales;
namespace SimpleEcommerce.Api.Dtos.Sales
{
    public class SalesMappingProfile : Profile
    {
        public SalesMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(x => x.User, opt => opt.MapFrom(c => c.User))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.Product, opt => opt.MapFrom(c => c.Product));
        }
    }
}
