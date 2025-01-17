using FluentValidation;
using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.EntityFramework;

namespace SimpleEcommerce.Api.Models.Catalog
{
    public class ProductPictureModel
    {
        public string PictureId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductPictureModelValidator : AbstractValidator<ProductPictureModel>
    {
        private readonly IRepository<Picture> _pictureRepository;
        public ProductPictureModelValidator(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository;

            RuleFor(x => x.PictureId)
                .MaximumLength(450)
                .MustAsync(CheckPictureExistance)
                .WithMessage("Invalid picture id.")
                .MustAsync(CheckPictureType)
                .WithMessage("Invalid picture type.");


            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(30);
        }

        private async Task<bool> CheckPictureExistance(string pictureId , CancellationToken cancellationToken)
        {
            return await _pictureRepository.AnyAsync(x => x.Id == pictureId);
        }

        private async Task<bool> CheckPictureType(string pictureId, CancellationToken cancellationToken)
        {
            var picture = await _pictureRepository.SingleAsync(x => x.Id == pictureId);

            return picture.PictureType == PictureType.Entity;
        }
    }
}
