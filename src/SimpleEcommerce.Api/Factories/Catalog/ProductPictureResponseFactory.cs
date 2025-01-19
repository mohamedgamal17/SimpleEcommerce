using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Factories.Media;

namespace SimpleEcommerce.Api.Factories.Catalog
{
    public class ProductPictureResponseFactory : ResponseFactory<ProductPicture, ProductPictureDto>
    {
        private readonly MediaResponseFactory _mediaResponseFactory;

        public ProductPictureResponseFactory(MediaResponseFactory mediaResponseFactory)
        {
            _mediaResponseFactory = mediaResponseFactory;
        }

        public override async Task<ProductPictureDto> PrepareDto(ProductPicture data)
        {
            var dto = new ProductPictureDto
            {
                Id = data.Id,
                PictureId = data.ProductId,
                ProductId = data.ProductId,
                DisplayOrder = data.DisplayOrder,
                Picture = await _mediaResponseFactory.PrepareDto(data.Picture)
            };

            return dto;
        }
    }
}
