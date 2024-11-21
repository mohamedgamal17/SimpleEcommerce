using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Domain.Users;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Users
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).HasMaxLength(256);
            builder.Property(x => x.LastName).HasMaxLength(256);

            builder.HasMany<Address>(x => x.Addresses).WithOne();
        }
    }
}
