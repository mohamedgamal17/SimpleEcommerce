using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Domain.Cart;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Cart
{
    public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).HasMaxLength(256);

            builder.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.BasketId);

            builder.HasIndex(x => x.UserId);
        }
    }
}
