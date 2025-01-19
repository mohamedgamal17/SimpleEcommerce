namespace SimpleEcommerce.Api.Domain
{
    public abstract class BaseEntity
    {

    }
    public abstract class Entity<TId> : BaseEntity
    {
        public TId Id { get; set; }
    }
    public abstract class Entity : Entity<string>
    {
        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
