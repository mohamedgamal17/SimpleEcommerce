using FluentValidation;

namespace SimpleEcommerce.Api.Models.Catalog
{
    public class BrandModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class BrandModelValidator : AbstractValidator<BrandModel>
    {
        public BrandModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(265);

            RuleFor(x => x.Description)
                .MaximumLength(600)
                .Unless(x => x.Description != null);
        }
    }
}
