using FluentValidation;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.EntityFramework;

namespace SimpleEcommerce.Api.Models.Sales
{
    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderItemModelValidator : AbstractValidator<OrderItemModel>
    {
        private readonly IRepository<Product> _productRepository;
        public OrderItemModelValidator(IRepository<Product> productRepository)
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .NotNull()
                .MaximumLength(450)
                .MustAsync(CheckProductId)
                .WithMessage("Product id should be valid");


            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            _productRepository = productRepository;
        }

        private async Task<bool> CheckProductId(string productId , CancellationToken cancellationToken)
        {
            return await _productRepository.AnyAsync(x => x.Id == productId);
        }
    }
}
