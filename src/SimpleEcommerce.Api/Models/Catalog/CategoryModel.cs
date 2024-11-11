using FluentValidation;

namespace SimpleEcommerce.Api.Models.Catalog
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class CategoryModelValidator : AbstractValidator<CategoryModel>
    {
        public CategoryModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(256);

            RuleFor(x => x.Description)
                .MaximumLength(600)
                .Unless(x => x.Description != null);
        }

    }
}
