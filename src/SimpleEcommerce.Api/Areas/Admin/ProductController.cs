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

namespace SimpleEcommerce.Api.Areas.Admin.Controllers
{
    [Route("api/[area]/products")]
    [ApiController]
    public class ProductController : AdminController
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public ProductController(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IRepository<Brand> brandRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedDto<ProductDto>))]
        public async Task<PagedDto<ProductDto>> GetProductsPaged(int skip = 0, int limit = 10)
        {
            var query = _productRepository.AsQuerable()
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

            return await query.OrderBy(x=> x.Id).ToPaged(skip, limit);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ProductDto> GetProduct(int id)
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

        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ProductDto> CreateProduct([FromBody] ProductModel model)
        {
            var nameExist = await _productRepository.AnyAsync(x => x.Name == model.Name);

            if (nameExist)
            {
                throw new BusinessLogicException($"Product name : ${model.Name} , is already exist choose another name");
            }

            var product = new Product();

            await PreapreProduct(product, model);
       
            await _productRepository.InsertAsync(product);


            return await _productRepository.AsQuerable()
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .SingleAsync(x => x.Id == product.Id);
        }


        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ProductDto> UpdateProduct(int id, [FromBody] ProductModel model)
        {
            var product = await _productRepository.SingleOrDefaultAsync(x => x.Id == id);

            if(product == null)
            {
                throw new EntityNotFoundException(typeof(Product), id);
            }

            var nameExist = await _productRepository.AnyAsync(x => x.Name == model.Name && x.Id != id);

            if (nameExist)
            {
                throw new BusinessLogicException($"Product name : ${model.Name} , is already exist choose another name");
            }

            await PreapreProduct(product, model);


            return await _productRepository.AsQuerable()
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .SingleAsync(x => x.Id == product.Id);
        }

        private async Task PreapreProduct(Product product , ProductModel model)
        {
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;


            if (model.Categories != null)
            {
                if (model.Categories.Count > 0)
                {
                    var categories = await _categoryRepository.AsQuerable()
                    .Where(x => model.Categories.Contains(x.Id))
                    .ToListAsync();

                    product.ProductCategories = categories.Select(x => new ProductCategory
                    {
                        CategoryId = x.Id

                    }).ToList();

                }
                else
                {
                    product.ProductCategories = new List<ProductCategory>();
                }

            }
            if (model.Brands != null)
            {
                if (model.Brands.Count > 0)
                {
                    var brands = await _brandRepository.AsQuerable()
                    .Where(x => model.Brands.Contains(x.Id))
                    .ToListAsync();

                    product.ProductBrands = brands.Select(x => new ProductBrand
                    {
                        BrandId = x.Id

                    }).ToList();

                }
                else
                {
                    product.ProductBrands = new List<ProductBrand>();
                }
            }

        }

    }
}
