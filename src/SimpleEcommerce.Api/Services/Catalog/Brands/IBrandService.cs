using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
namespace SimpleEcommerce.Api.Services.Catalog.Brands
{
    public interface IBrandService
    {
        Task<BrandDto> CreateAsync(BrandModel model, CancellationToken cancellationToken = default);
        Task<BrandDto> UpdateAsync(string id, BrandModel model, CancellationToken cancellationToken = default);
        Task<PagedDto<BrandDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default);
        Task<BrandDto> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
