using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos.Catalog;
namespace SimpleEcommerce.Api.Factories.Catalog
{
    public class CategoryResponseFactory : ResponseFactory<Category, CategoryDto>
    {    
        public override Task<CategoryDto> PrepareDto(Category category)
        {
            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return Task.FromResult(dto);
        }
    }
}
