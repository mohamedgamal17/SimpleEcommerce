using FluentValidation;

namespace SimpleEcommerce.Api.Models.Common
{
    public class PagingModel
    {
        public int Skip { get; set; }
        public int Limit { get; set; }

        public PagingModel()
        {
            Skip = 0;
            Limit = 10;
        }
    }

    public class PagingModelValidator : AbstractValidator<PagingModel>
    {
        public PagingModelValidator()
        {
            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .LessThanOrEqualTo(150);
                
        }
    }
}
