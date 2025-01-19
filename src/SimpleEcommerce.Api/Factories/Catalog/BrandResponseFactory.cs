using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos.Catalog;
namespace SimpleEcommerce.Api.Factories.Catalog
{
    public class BrandResponseFactory : ResponseFactory<Brand, BrandDto>
    {
        public override Task<BrandDto> PrepareDto(Brand data)
        {
            var dto = new BrandDto
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description
            };

            return Task.FromResult(dto);
        }
    }
}
