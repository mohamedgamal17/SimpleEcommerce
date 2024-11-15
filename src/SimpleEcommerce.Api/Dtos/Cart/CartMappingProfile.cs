using AutoMapper;
using SimpleEcommerce.Api.Domain.Cart;

namespace SimpleEcommerce.Api.Dtos.Cart
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<Basket, BasketDto>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(x => x.Product, opt => opt.MapFrom(c => c.Product));
        }
    }
}
