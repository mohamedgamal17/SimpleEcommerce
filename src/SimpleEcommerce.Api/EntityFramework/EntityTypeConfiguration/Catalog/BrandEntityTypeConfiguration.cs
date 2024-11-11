using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Domain.Catalog;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Catalog
{
    public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(600).IsRequired(false);
            builder.HasMany<ProductBrand>().WithOne(x=> x.Brand).HasForeignKey(x => x.BrandId);
        }
    }
}

