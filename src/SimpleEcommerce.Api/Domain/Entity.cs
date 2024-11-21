namespace SimpleEcommerce.Api.Domain
{
    public class Entity<TId>
    {
        public TId Id { get; set; }
    }
    public class Entity : Entity<int>
    {
    }
}
