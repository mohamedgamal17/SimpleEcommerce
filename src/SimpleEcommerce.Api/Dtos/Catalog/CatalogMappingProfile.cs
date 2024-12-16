using AutoMapper;
using SimpleEcommerce.Api.Domain.Catalog;

namespace SimpleEcommerce.Api.Dtos.Catalog
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<Brand, BrandDto>();

            CreateMap<ProductBrand, ProductBrandDto>()
                .ForMember(x => x.Brand, opt => opt.MapFrom(c => c.Brand));

            CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(x => x.Category, opt => opt.MapFrom(c => c.Category));

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(c => c.ProductCategories))
                .ForMember(x => x.ProductBrands, opt => opt.MapFrom(c => c.ProductBrands))
                .ForMember(x => x.Pictures, opt => opt.MapFrom(c => c.ProductPictures));
        }
    }
}
