using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Factories.Catalog;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Services.Catalog.Categories;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<CategoryDto>))]
        public async Task<PagedDto<CategoryDto>> GetCategoriesPaged([FromQuery]PagingModel model)
        {        
            var resposne = await _categoryService.ListPagedAsync(model);

            return resposne;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> GetCategory(string id)
        {

            var resposne = await _categoryService.GetAsync(id);

            return resposne;
        }

    }
}
