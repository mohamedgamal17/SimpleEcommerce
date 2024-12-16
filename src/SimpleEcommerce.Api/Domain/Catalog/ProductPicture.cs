using SimpleEcommerce.Api.Domain.Media;

namespace SimpleEcommerce.Api.Domain.Catalog
{
    public class ProductPicture : Entity
    {
        public int ProductId { get; set; }
        public int PictureId { get; set; }
        public int DisplayOrder { get; set; }
        public Picture Picture { get; set; }
    }
}
