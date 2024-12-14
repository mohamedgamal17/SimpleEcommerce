using FluentValidation;

namespace SimpleEcommerce.Api.Models.Cart
{
    public class BasketModel
    {
        public List<BasketItemModel> Items { get; set;}

        public BasketModel()
        {
            Items = new List<BasketItemModel>();
        }
    }

    public class BasketModelValidator: AbstractValidator<BasketModel>
    {
        private readonly IServiceProvider _serviceProvider;
        public BasketModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;


            RuleFor(x => x.Items)
                .NotNull();

            RuleForEach(x => x.Items)
                .SetValidator(_serviceProvider.GetRequiredService<BasketItemModelValidator>())
                .When(x => x.Items.Count > 0);
        }
    }
}
