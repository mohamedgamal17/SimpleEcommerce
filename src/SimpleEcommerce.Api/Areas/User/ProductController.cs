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
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<ProductDto>))]
        public async Task<PagedDto<ProductDto>> GetProductsPaged(int skip = 0, int limit = 10)
        {
            var query = _productRepository.AsQuerable()
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

            return await query.OrderBy(x => x.Id).ToPaged(skip, limit);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ProductDto> GetProduct(string id)
        {
            var query = _productRepository.AsQuerable()
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Product), id);
            }

            return result;
        }

        [HttpGet("{productId}/pictures")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductPictureDto>))]
        public async Task<List<ProductPictureDto>> GetProductPictures(string productId)
        {
            var product = await _productRepository.SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            return _mapper.Map<List<ProductPicture>, List<ProductPictureDto>>(product.ProductPictures);
        }

        [HttpGet("{productId}/pictures/{pictureId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductPictureDto>))]
        public async Task<ProductPictureDto> GetProductPicture(string productId, string pictureId)
        {
            var product = await _productRepository.SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var productPicture = product.ProductPictures.SingleOrDefault(x => x.Id == pictureId);

            if (productPicture == null)
            {
                throw new EntityNotFoundException(typeof(ProductPicture), pictureId);
            }

            return _mapper.Map<ProductPicture, ProductPictureDto>(productPicture);
        }


    }
}
