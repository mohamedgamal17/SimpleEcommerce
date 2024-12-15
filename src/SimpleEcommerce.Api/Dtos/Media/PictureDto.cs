using SimpleEcommerce.Api.Domain.Media;

namespace SimpleEcommerce.Api.Dtos.Media
{
    public class PictureDto : EntityDto
    {
        public string MimeType { get; set; }
        public string AltAttribute { get; set; }
        public string Url { get; set; }
        public PictureType PictureType { get; set; }
    }
}
