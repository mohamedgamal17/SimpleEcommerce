using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos.Catalog;
namespace SimpleEcommerce.Api.Factories.Catalog
{
    public class ProductResponseFactory : ResponseFactory<Product, ProductDto>
    {
        private readonly ProductPictureResponseFactory _productPictureResponseFactory;
        private readonly BrandResponseFactory _brandResponseFactory;
        private readonly CategoryResponseFactory _categoryResponseFactory;
        public ProductResponseFactory(ProductPictureResponseFactory productPictureResponseFactory, BrandResponseFactory brandResponseFactory, CategoryResponseFactory categoryResponseFactory)
        {
            _productPictureResponseFactory = productPictureResponseFactory;
            _brandResponseFactory = brandResponseFactory;
            _categoryResponseFactory = categoryResponseFactory;
        }

        public override async Task<ProductDto> PrepareDto(Product data)
        {
            var dto = new ProductDto
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
            };

            if (data.ProductPictures != null)
            {
                dto.Pictures = await _productPictureResponseFactory.PrepareListDto(data.ProductPictures);
            }

            if (data.ProductBrands != null)
            {
                dto.ProductBrands = await data.ProductBrands.ToAsyncEnumerable().SelectAwait(async x => new ProductBrandDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    ProductId = x.ProductId,
                    Brand = await _brandResponseFactory.PrepareDto(x.Brand)
                }).ToListAsync();
            }

            if (data.ProductCategories != null)
            {
                dto.ProductCategories = await data.ProductCategories
                    .ToAsyncEnumerable()
                    .SelectAwait(async x => new ProductCategoryDto
                    {
                        Id = x.Id,
                        CategoryId = x.CategoryId,
                        ProductId = x.ProductId,
                        Category = await _categoryResponseFactory.PrepareDto(x.Category)
                    }).ToListAsync();
            }


            return dto;
        }
    }
}
