using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Domain.Catalog;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Catalog
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(600).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1500).IsRequired();

            builder.HasMany(x => x.ProductCategories).WithOne().HasForeignKey(x => x.ProductId);
            builder.HasMany(x => x.ProductBrands).WithOne().HasForeignKey(x => x.ProductId);
        }
    }
}

