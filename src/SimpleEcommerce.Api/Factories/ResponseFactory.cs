using SimpleEcommerce.Api.Dtos;

namespace SimpleEcommerce.Api.Factories
{
    public abstract class ResponseFactory<TEntity, TDto> : IResponseFactory<TEntity, TDto>
    {
        public async Task<PagedDto<TDto>> PreparePagedDto(PagedDto<TEntity> data)
        {
            var paged = new PagedDto<TDto>
            {
                Data = await PrepareListDto(data.Data),
                Skip = data.Skip,
                Length = data.Length,
                TotalCount = data.TotalCount
            };

            return paged;
        }

        public async Task<List<TDto>> PrepareListDto(List<TEntity> data)
        {
            var tasks = data.Select(PrepareDto);

            return (await Task.WhenAll(tasks)).ToList();
        }

        public abstract Task<TDto> PrepareDto(TEntity data);
    }
}
