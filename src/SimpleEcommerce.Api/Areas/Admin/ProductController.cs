using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Services.Catalog.Products;
namespace SimpleEcommerce.Api.Areas.Admin
{
    [Route("api/[area]/products")]
    [Authorize]
    [ApiController]
    public class ProductController : AdminController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<ProductDto>))]
        public async Task<PagedDto<ProductDto>> GetProductsPaged([FromQuery] PagingModel model)
        {
            var response = await _productService.ListPagedAsync(model);

            return response;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ProductDto> GetProduct(string id)
        {
            var response = await _productService.GetAsync(id);

            return response;
        }


        [HttpGet("{productId}/pictures")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<ProductPictureDto>))]
        public async Task<PagedDto<ProductPictureDto>> GetProductPictures(string productId , [FromQuery] PagingModel model)
        {
            var response = await _productService.ListProductPicturePagedAsync(productId, model);

            return response;
        }

        [HttpGet("{productId}/pictures/{pictureId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductPictureDto>))]
        public async Task<ProductPictureDto> GetProductPicture(string productId , string pictureId)
        {
            var response = await _productService.GetProductPictureAsync(productId, pictureId);

            return response;
        }

        [HttpPost("{productId}/pictures")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductPictureDto))]
        public async Task<ProductPictureDto> CreateProductPicture(string productId, [FromBody] ProductPictureModel model)
        {
            var response = await _productService.CreateProductPictureAsync(productId, model);

            return response;
        }

        [HttpDelete("{productId}/pictures/{pictureId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task RemoveProductPicture(string productId , string pictureId)
        {
            await _productService.RemoveProductPictureAsync(productId, pictureId);

        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ProductDto> CreateProduct([FromBody] ProductModel model)
        {
            var response = await _productService.CreateAsync(model);

            return response;
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ProductDto> UpdateProduct(string id, [FromBody] ProductModel model)
        {
            var response = await _productService.UpdateAsync(id, model);

            return response;
        }


    }
}
