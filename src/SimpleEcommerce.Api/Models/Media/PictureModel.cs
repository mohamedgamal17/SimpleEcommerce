using FluentValidation;

namespace SimpleEcommerce.Api.Models.Media
{
    public class PictureModel
    {
        public string? AltAttribute { get; set; }
        public IFormFile Image { get; set; }
    }

    public class PictureModelValidator : AbstractValidator<PictureModel>
    {
        public PictureModelValidator()
        {
            RuleFor(x => x.AltAttribute)
                .MaximumLength(256)
                .When(x => x.AltAttribute != null);


            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Image file is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Image.ContentType)
                        .Must(IsSupportedContentType)
                        .WithMessage("Unsupported file type. Only JPEG and PNG are allowed.");


                    RuleFor(x => x.Image.Length)
                    .LessThanOrEqualTo(2 * 1024 * 1024)
                    .WithMessage("File size must be less than 2 MB.");
                });
                
        }

        private bool IsSupportedContentType(string contentType)
        {
            return contentType == "image/jpeg"
                || contentType == "image/png"
                || contentType == "image/apng"
                || contentType == "image/avif"
                || contentType == "image/gif"
                || contentType == "image/svg+xml"
                || contentType == "image/webp";
        }
    }


}
