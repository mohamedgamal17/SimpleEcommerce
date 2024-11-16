using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Domain.Cart;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Cart
{
    public class BasketItemEntityTypeConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        }
    }
}
