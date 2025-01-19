using AutoMapper.QueryableExtensions;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
using System.Collections.Generic;

namespace SimpleEcommerce.Api.Services.Catalog.Categories
{
    public interface ICategoryService : IApplicationService
    {
        Task<CategoryDto> CreateAsync(CategoryModel model , CancellationToken cancellationToken = default);
        Task<CategoryDto> UpdateAsync(string id, CategoryModel model, CancellationToken cancellationToken = default);
        Task<PagedDto<CategoryDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default);
        Task<CategoryDto> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
