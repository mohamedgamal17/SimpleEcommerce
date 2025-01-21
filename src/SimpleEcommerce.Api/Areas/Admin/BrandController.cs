using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Services.Catalog.Brands;
namespace SimpleEcommerce.Api.Areas.Admin
{
    [Route("api/[area]/brands")]
    [Authorize]
    [ApiController]
    public class BrandController : AdminController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<CategoryDto>))]
        public async Task<PagedDto<BrandDto>> GetBrandsPaged([FromQuery]PagingModel model)
        {
            var response = await _brandService.ListPagedAsync(model);

            return response;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        public async Task<BrandDto> GetBrand(string id)
        {
            var response = await _brandService.GetAsync(id);

            return response;
        }

        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        public async Task<BrandDto> CreateBrand([FromBody] BrandModel model)
        {
            var response = await _brandService.CreateAsync(model);

            return response;
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        public async Task<BrandDto> UpdateBrand(string id, [FromBody] BrandModel model)
        {
            var response = await _brandService.UpdateAsync(id, model);

            return response;
        }
    }
}
