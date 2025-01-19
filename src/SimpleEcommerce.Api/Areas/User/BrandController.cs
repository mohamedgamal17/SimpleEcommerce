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
using SimpleEcommerce.Api.Models.Catalog;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;
        public BrandController(IRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<CategoryDto>))]
        public async Task<PagedDto<BrandDto>> GetBrandsPaged(int skip = 0, int limit = 10)
        {
            var query = _brandRepository.AsQuerable()
                .ProjectTo<BrandDto>(_mapper.ConfigurationProvider);

            var result = await query.ToPaged(skip, limit);

            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        public async Task<BrandDto> GetBrand(string id)
        {
            var query = _brandRepository.AsQuerable()
               .ProjectTo<BrandDto>(_mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Brand), id);
            }

            return result;
        }

    }
}
