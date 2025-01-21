using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Services.Catalog.Categories;
namespace SimpleEcommerce.Api.Areas.Admin
{
    [Route("api/[area]/categories")]
    [Authorize]
    [ApiController]
    public class CategoryController : AdminController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<CategoryDto>))]
        public async Task<PagedDto<CategoryDto>> GetCategoriesPaged([FromQuery] PagingModel model)
        {
            var response = await _categoryService.ListPagedAsync(model);

            return response;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> GetCategory(string id)
        {
            var response = await _categoryService.GetAsync(id);

            return response;
        }

        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> CreateCategory([FromBody] CategoryModel model)
        {
            var response = await _categoryService.CreateAsync(model);

            return response;
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> UpdateCategory(string id, [FromBody] CategoryModel model)
        {
            var response = await _categoryService.UpdateAsync(id, model);

            return response;
        }

    }
}
