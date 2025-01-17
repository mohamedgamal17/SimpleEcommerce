namespace SimpleEcommerce.Api.Dtos
{
    public class EntityDto<TId>
    {
        public TId Id { get; set; }
    }
    public class EntityDto : EntityDto<string>
    {
    }
}
