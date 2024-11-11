namespace SimpleEcommerce.Api.Dtos
{
    public class PagedDto<T>
    {
        public List<T> Data { get; set; }
        public int Skip { get; set; }
        public int Length { get; set; }
        public long TotalCount { get; set; }

        public PagedDto()
        {

        }
        public PagedDto(List<T> data, long count, int skip, int length)
        {
            Data = data;
            TotalCount = count;
            Skip = skip;
            Length = length;

        }
    }
}
