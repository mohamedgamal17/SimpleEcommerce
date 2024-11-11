using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Models.Catalog;
namespace SimpleEcommerce.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly EcommerceDbContext _ecommerceDbContext;

        public CategoryController(IRepository<Category> categoryRepository, EcommerceDbContext ecommerceDbContext)
        {
            _categoryRepository = categoryRepository;
            _ecommerceDbContext = ecommerceDbContext;
        }


        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(PagedDto<CategoryDto>))]
        public async Task<PagedDto<CategoryDto>> GetCategoriesPaged(int skip = 0 ,int limit = 10)
        {
            var query = PrepareCategoryDtoQuery(_categoryRepository.AsQuerable());

            var result = await query.ToPaged(skip, limit);

            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> GetCategory(int id)
        {
            var query = PrepareCategoryDtoQuery(_categoryRepository.AsQuerable());

            var result = await query.SingleOrDefaultAsync(x => x.Id == id);

            if(result == null)
            {
                throw new EntityNotFoundException(typeof(Category), id);
            }

            return result;
        }

        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> CreateCategory([FromBody]CategoryModel model)
        {
            var nameExist = await _categoryRepository.AnyAsync(x => x.Name == model.Name);

            if (nameExist)
            {
                throw new BusinessLogicException($"Category name : ${model.Name} , is already exist choose another name");
            }

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            await _categoryRepository.InsertAsync(category);

            var query = PrepareCategoryDtoQuery(_categoryRepository.AsQuerable());

            return await query.SingleAsync(x => x.Id == category.Id);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> UpdateCategory(int id, [FromBody] CategoryModel model)
        {
            var category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == id);

            if(category == null)
            {
                throw new EntityNotFoundException(typeof(Category), id);
            }

            var nameExist = await _categoryRepository.AnyAsync(x => x.Name == model.Name && x.Id != id);

            if (nameExist)
            {
                throw new BusinessLogicException($"Category name : ${model.Name} , is already exist choose another name");
            }

            category.Name = model.Name;
            category.Description = model.Description;

            await _categoryRepository.UpdateAsync(category);

            var query = PrepareCategoryDtoQuery(_categoryRepository.AsQuerable());

            return await query.SingleAsync(x => x.Id == category.Id);
        }
        private IQueryable<CategoryDto> PrepareCategoryDtoQuery(IQueryable<Category> query)
        {
            var newQuery = query.Select(x=> new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });

            return newQuery;
        }
    }
}
