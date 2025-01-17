using SimpleEcommerce.Api.Dtos.Media;

namespace SimpleEcommerce.Api.Dtos.Catalog
{
    public class ProductPictureDto : EntityDto
    {
        public string PictureId { get; set; }
        public string ProductId { get; set; }
        public int DisplayOrder { get; set; }
        public PictureDto Picture { get; set; }
    }
}
