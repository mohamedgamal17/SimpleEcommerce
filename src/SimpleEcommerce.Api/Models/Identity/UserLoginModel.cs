using FluentValidation;

namespace SimpleEcommerce.Api.Models.Identity
{
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
