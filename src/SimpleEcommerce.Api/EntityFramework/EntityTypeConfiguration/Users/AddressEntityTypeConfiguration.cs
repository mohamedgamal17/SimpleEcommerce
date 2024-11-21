using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Domain.Users;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Users
{
    public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).HasMaxLength(256);
            builder.Property(x => x.LastName).HasMaxLength(256);
            builder.Property(x => x.Email).HasMaxLength(256);
            builder.Property(x => x.Phone).HasMaxLength(50);
            builder.Property(x => x.City).HasMaxLength(50);
            builder.Property(x => x.Zip).HasMaxLength(50);
            builder.Property(x => x.AddressLine1).HasMaxLength(500);
            builder.Property(x => x.AddressLine2).HasMaxLength(500);
        }
    }
}
