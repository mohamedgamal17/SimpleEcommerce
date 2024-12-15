using SimpleEcommerce.Api.Domain;

namespace SimpleEcommerce.Api.Models.Media
{
    public class Picture : Entity
    {
        public string MimeType { get; set; }
        public string AltAttribute { get; set; }
        public string S3Key { get; set; }
    }
}
