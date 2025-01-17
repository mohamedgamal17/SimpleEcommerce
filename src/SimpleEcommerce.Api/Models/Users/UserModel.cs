using FluentValidation;
using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.EntityFramework;

namespace SimpleEcommerce.Api.Models.Users
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string? AvatarId { get; set; }
        public List<AddressModel>? Addresses { get; set; } 
    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        private readonly IRepository<Picture> _pictureRepository;
        public UserModelValidator(IRepository<Picture> pictureRepository)
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(256)
                .MinimumLength(2)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.LastName)
                .MaximumLength(256)
                .MinimumLength(2)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.BirthDate).NotEmpty().NotNull();

            RuleFor(x => x.Gender).IsInEnum();

            RuleFor(x => x.AvatarId)
                .NotEmpty()
                .NotNull()
                .MaximumLength(450)
                .MustAsync(CheckPictureExistance)
                .WithMessage("Invalid picture id.")
                .MustAsync(CheckPictureType)
                .WithMessage("Invalid picture type.")
                .When(x => x.AvatarId != null);


            RuleForEach(x => x.Addresses)
                .SetValidator(new AddressModelValidator())
                .When(x => x.Addresses != null);
            _pictureRepository = pictureRepository;
        }
        private async Task<bool> CheckPictureExistance(string pictureId, CancellationToken cancellationToken)
        {
            return await _pictureRepository.AnyAsync(x => x.Id == pictureId);
        }

        private async Task<bool> CheckPictureType(string pictureId, CancellationToken cancellationToken)
        {
            var picture = await _pictureRepository.SingleAsync(x => x.Id == pictureId);

            return picture.PictureType == PictureType.Avatar;
        }

    }
}
