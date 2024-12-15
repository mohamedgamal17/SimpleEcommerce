using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleEcommerce.Api.Models.Media;

namespace SimpleEcommerce.Api.EntityFramework.EntityTypeConfiguration.Media
{
    public class PictureEntityTypeConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.Property(x => x.MimeType).HasMaxLength(256);
            builder.Property(x => x.AltAttribute).IsRequired(false).HasMaxLength(256);
            builder.Property(x => x.S3Key).HasMaxLength(500);
        }
    }
}
