using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Models.Sales;
namespace SimpleEcommerce.Api.Services.Sales
{
    public interface IOrderSerivce : IApplicationService
    {
        Task<PagedDto<OrderDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default);
        Task<OrderDto> GetAsync(string orderId, CancellationToken cancellationToken = default);
        Task<PagedDto<OrderDto>> ListUserOrdersPagedAsync(string userId,PagingModel model ,CancellationToken cancellationToken = default);
        Task<OrderDto> GetUserOrderAsync(string userId,string orderId ,CancellationToken cancellationToken = default);
        Task<OrderDto> CreateAsync(OrderModel model, CancellationToken cancellationToken = default);
    }
}
