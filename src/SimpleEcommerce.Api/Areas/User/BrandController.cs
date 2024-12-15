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
        public async Task<BrandDto> GetBrand(int id)
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

        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        public async Task<BrandDto> CreateBrand([FromBody] BrandModel model)
        {
            var nameExist = await _brandRepository.AnyAsync(x => x.Name == model.Name);

            if (nameExist)
            {
                throw new BusinessLogicException($"Brand name : ${model.Name} , is already exist choose another name");
            }

            var brand = new Brand
            {
                Name = model.Name,
                Description = model.Description
            };
            await _brandRepository.InsertAsync(brand);

            var result = await _brandRepository.AsQuerable()
               .ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
               .SingleAsync(x => x.Id == brand.Id);


            return result;
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        public async Task<BrandDto> UpdateBrand(int id, [FromBody] BrandModel model)
        {
            var brand = await _brandRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (brand == null)
            {
                throw new EntityNotFoundException(typeof(Brand), id);
            }

            var nameExist = await _brandRepository.AnyAsync(x => x.Name == model.Name && x.Id != id);

            if (nameExist)
            {
                throw new BusinessLogicException($"Brand name : ${model.Name} , is already exist choose another name");
            }

            brand.Name = model.Name;
            brand.Description = model.Description;

            await _brandRepository.UpdateAsync(brand);

            return await _brandRepository.AsQuerable()
               .ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
               .SingleAsync(x => x.Id == brand.Id);
        }
    }
}
