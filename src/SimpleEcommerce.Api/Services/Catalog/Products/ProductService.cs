using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Factories.Catalog;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;

namespace SimpleEcommerce.Api.Services.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly ProductResponseFactory _productResponseFactory;
        private readonly ProductPictureResponseFactory _productPictureResponseFactory;

        public ProductService(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IRepository<Brand> brandRepository, IRepository<ProductPicture> productPictureRepository, ProductResponseFactory productResponseFactory, ProductPictureResponseFactory productPictureResponseFactory)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productPictureRepository = productPictureRepository;
            _productResponseFactory = productResponseFactory;
            _productPictureResponseFactory = productPictureResponseFactory;
        }

        public async Task<ProductDto> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            var nameExist = await _productRepository.AnyAsync(x => x.Name == model.Name);

            if (nameExist)
            {
                throw new BusinessLogicException($"Product name : ${model.Name} , is already exist choose another name");
            }

            var product = new Product();

            await PreapreProduct(product, model);

            await _productRepository.InsertAsync(product);

            var query = PrepareProductQuery();

            var result = await query.SingleAsync(x => x.Id == product.Id, cancellationToken);

            var response = await _productResponseFactory.PrepareDto(result);

            return response;
        }

        public async Task<ProductDto> UpdateAsync(string productId, ProductModel model, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var nameExist = await _productRepository.AnyAsync(x => x.Name == model.Name && x.Id != productId);

            if (nameExist)
            {
                throw new BusinessLogicException($"Product name : ${model.Name} , is already exist choose another name");
            }

            await PreapreProduct(product, model);

            var result = await PrepareProductQuery()
                .SingleAsync(x => x.Id == product.Id, cancellationToken);

            var response = await _productResponseFactory.PrepareDto(result);

            return response;
        }


        public async Task<PagedDto<ProductDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default)
        {
            var result = await PrepareProductQuery()
                .OrderBy(x => x.Id)
                .ToPaged(model.Skip, model.Limit);

            var response = await _productResponseFactory.PreparePagedDto(result);

            return response;
        }
        public async Task<ProductDto> GetAsync(string productId, CancellationToken cancellationToken = default)
        {
            var query = PrepareProductQuery();

            var result = await query.SingleOrDefaultAsync(x => x.Id == productId);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var resposne = await _productResponseFactory.PrepareDto(result);

            return resposne;
        }

        public async Task<ProductPictureDto> CreateProductPictureAsync(string productId,ProductPictureModel model ,CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var productPicture = new ProductPicture
            {
                PictureId = model.PictureId,
                ProductId = product.Id,
                DisplayOrder = model.DisplayOrder
            };

            product.ProductPictures.Add(productPicture);


            await _productRepository.UpdateAsync(product);

            productPicture = await _productPictureRepository.SingleAsync(x => x.Id == productPicture.Id);

            var response = await _productPictureResponseFactory.PrepareDto(productPicture);

            return response;
        }


        public async Task<ProductPictureDto> UpdateProductPictureAsync(string productId, string pictureId,ProductPictureModel model ,CancellationToken cancellationToken = default)
        {
            var isProductExist = await _productRepository.AnyAsync(x => x.Id == productId);

            if (!isProductExist)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var productPicture = await _productPictureRepository.SingleOrDefaultAsync(x => x.ProductId == productId && x.Id == pictureId);

            if(productPicture == null)
            {
                throw new EntityNotFoundException(typeof(ProductPicture),pictureId);
            }

            productPicture.PictureId = model.PictureId;
            productPicture.DisplayOrder = model.DisplayOrder;

            await _productPictureRepository.UpdateAsync(productPicture);


            var response = await _productPictureResponseFactory.PrepareDto(productPicture);

            return response;
        }
        public async Task RemoveProductPictureAsync(string productId, string pictureId, CancellationToken cancellationToken = default)
        {
            var isProductExist = await _productRepository.AnyAsync(x => x.Id == productId);

            if (!isProductExist)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var productPicture = await _productPictureRepository.SingleOrDefaultAsync(x => x.ProductId == productId && x.Id == pictureId);

            if (productPicture == null)
            {
                throw new EntityNotFoundException(typeof(ProductPicture), pictureId);
            }

            await _productPictureRepository.DeleteAsync(productPicture);

        }
        public async Task<PagedDto<ProductPictureDto>> ListProductPicturePagedAsync(string productId, PagingModel model, CancellationToken cancellationToken = default)
        {
            var isProductExist = await _productRepository.AnyAsync(x => x.Id == productId);

            if (!isProductExist)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var pictures = await _productPictureRepository.AsQuerable()
                .Where(x => x.ProductId == productId)
                .Include(c => c.Picture)
                .OrderBy(x => x.Id)
                .ToPaged(model.Skip, model.Limit);

            var resposne = await _productPictureResponseFactory.PreparePagedDto(pictures);

            return resposne;
        }

        public async Task<ProductPictureDto> GetProductPictureAsync(string productId, string pictureId, CancellationToken cancellationToken = default)
        {
            var isProductExist = await _productRepository.AnyAsync(x => x.Id == productId);

            if (!isProductExist)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var productPicture = await _productPictureRepository.AsQuerable()
                .Include(c => c.Picture)
                .SingleOrDefaultAsync(x => x.ProductId == productId && x.Id == pictureId);

            if (productPicture == null)
            {
                throw new EntityNotFoundException(typeof(ProductPicture), pictureId);
            }

            var resposne = await _productPictureResponseFactory.PrepareDto(productPicture);

            return resposne;
        }


        private async Task PreapreProduct(Product product, ProductModel model)
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

            if (model.Pictures != null)
            {
                product.ProductPictures = model.Pictures.Select(x => new ProductPicture
                {
                    PictureId = x.PictureId,
                    DisplayOrder = x.DisplayOrder
                }).ToList();
            }

        }

        private IQueryable<Product> PrepareProductQuery()
        {
            return _productRepository.AsQuerable();
        }
    }
}
