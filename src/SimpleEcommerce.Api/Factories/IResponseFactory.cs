using SimpleEcommerce.Api.Dtos;

namespace SimpleEcommerce.Api.Factories
{
    public interface IResponseFactory<TEntity,TDto>
    {
        Task<PagedDto<TDto>> PreparePagedDto(PagedDto<TEntity> data);
        Task<List<TDto>> PrepareListDto(List<TEntity> data);
        Task<TDto> PrepareDto(TEntity data);
    }
}
