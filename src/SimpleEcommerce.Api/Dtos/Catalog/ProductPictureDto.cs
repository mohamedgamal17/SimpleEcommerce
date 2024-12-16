using SimpleEcommerce.Api.Dtos.Media;

namespace SimpleEcommerce.Api.Dtos.Catalog
{
    public class ProductPictureDto : EntityDto
    {
        public int PictureId { get; set; }
        public int ProductId { get; set; }
        public int DisplayOrder { get; set; }
        public PictureDto Picture { get; set; }
    }
}
