using SimpleEcommerce.Api.Domain.Media;

namespace SimpleEcommerce.Api.Domain.Catalog
{
    public class ProductPicture : Entity
    {
        public string ProductId { get; set; }
        public string PictureId { get; set; }
        public int DisplayOrder { get; set; }
        public Picture Picture { get; set; }
    }
}
