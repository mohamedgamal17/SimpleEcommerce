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
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(IRepository<Category> categoryRepository,  IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<CategoryDto>))]
        public async Task<PagedDto<CategoryDto>> GetCategoriesPaged(int skip = 0, int limit = 10)
        {
            var result = await _categoryRepository.AsQuerable()
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToPaged(skip, limit);

            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<CategoryDto> GetCategory(string id)
        {

            var result = await _categoryRepository.AsQuerable()
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Category), id);
            }

            return result;
        }

    }
}
