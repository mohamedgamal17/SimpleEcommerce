using FluentValidation;

namespace SimpleEcommerce.Api.Models.Identity
{
    public class UserRegisterModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}
