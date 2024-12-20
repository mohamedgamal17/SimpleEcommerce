using FluentValidation;

namespace SimpleEcommerce.Api.Models.Identity
{
    public class ChangePasswordModel 
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .MaximumLength(1000)
                .MinimumLength(5)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .MaximumLength(1000)
                .MinimumLength(5)
                .NotEmpty();
        }
    }
}
