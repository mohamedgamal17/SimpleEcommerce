using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
namespace SimpleEcommerce.Api.Services.Catalog.Products
{
    public interface IProductService : IApplicationService
    {
        Task<ProductDto> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);
        Task<ProductDto> UpdateAsync(string productId, ProductModel model, CancellationToken cancellationToken = default);
        Task<PagedDto<ProductDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default);
        Task<ProductDto> GetAsync(string productId, CancellationToken cancellationToken = default);
        Task<ProductPictureDto> CreateProductPictureAsync(string productId,ProductPictureModel model ,CancellationToken cancellationToken = default);
        Task<ProductPictureDto> UpdateProductPictureAsync(string productId, string pictureId,ProductPictureModel model ,CancellationToken cancellationToken = default);
        Task RemoveProductPictureAsync(string productId, string pictureId, CancellationToken cancellationToken = default);
        Task<PagedDto<ProductPictureDto>> ListProductPicturePagedAsync(string productId, PagingModel model, CancellationToken cancellationToken = default);
        Task<ProductPictureDto> GetProductPictureAsync(string productId, string pictureId, CancellationToken cancellationToken = default);
    }
}
