using SimpleEcommerce.Api.Domain;

namespace SimpleEcommerce.Api.Domain.Media
{
    public class Picture : Entity
    {
        public string MimeType { get; set; }
        public string AltAttribute { get; set; }
        public string S3Key { get; set; }
        public PictureType PictureType { get; set; }
    }
}
