using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Services.Catalog.Products;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
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
            var resposne = await _productService.GetAsync(id);

            return resposne;
        }

        [HttpGet("{productId}/pictures")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<ProductPictureDto>))]
        public async Task<PagedDto<ProductPictureDto>> GetProductPictures(string productId, [FromQuery] PagingModel model)
        {
            var resposne = await _productService.ListProductPicturePagedAsync(productId, model);

            return resposne;
        }

        [HttpGet("{productId}/pictures/{pictureId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductPictureDto>))]
        public async Task<ProductPictureDto> GetProductPicture(string productId, string pictureId)
        {
            var resposne = await _productService.GetProductPictureAsync(productId, pictureId);

            return resposne;
        }
    }
}
