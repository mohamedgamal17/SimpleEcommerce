using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Domain.Catalog;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Catalog
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(600).IsRequired(false);
            builder.HasMany<ProductCategory>().WithOne().HasForeignKey(x => x.CategoryId);

        }
    }
}

