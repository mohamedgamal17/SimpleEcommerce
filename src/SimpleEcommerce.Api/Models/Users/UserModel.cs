using FluentValidation;
using SimpleEcommerce.Api.Domain.Users;

namespace SimpleEcommerce.Api.Models.Users
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public List<AddressModel>? Addresses { get; set; } 
    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
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

            RuleForEach(x => x.Addresses)
                .SetValidator(new AddressModelValidator())
                .When(x => x.Addresses != null);
        }
    }
}
