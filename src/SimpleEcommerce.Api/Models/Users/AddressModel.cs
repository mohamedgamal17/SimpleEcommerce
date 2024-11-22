using FluentValidation;

namespace SimpleEcommerce.Api.Models.Users
{
    public class AddressModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string UserId { get; set; }
    }

    public class AddressModelValidator : AbstractValidator<AddressModel>
    {
        public AddressModelValidator()
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

            RuleFor(x => x.Email)
                .EmailAddress()
                .NotNull()
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Phone)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.City)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Zip)
                .MaximumLength(50)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.AddressLine1)
                .MaximumLength(500)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.AddressLine2)
              .MaximumLength(500)
              .NotEmpty()
              .NotNull();

            RuleFor(x => x.UserId)
              .MaximumLength(500)
              .NotEmpty()
              .NotNull();

        }
    }
}
