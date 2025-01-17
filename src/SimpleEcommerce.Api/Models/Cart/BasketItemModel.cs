using FluentValidation;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.EntityFramework;

namespace SimpleEcommerce.Api.Models.Cart
{
    public class BasketItemModel
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }

    public class BasketItemModelValidator : AbstractValidator<BasketItemModel>
    {
        private readonly IRepository<Product> _productRepository;
        public BasketItemModelValidator(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.ProductId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(450)
                .MustAsync(CheckProductExist)
                .WithMessage((_, productId) => $"Product with id : ({productId}) , is not exist");

        }

        private async Task<bool> CheckProductExist(string productId , CancellationToken cancellationToken = default)
        {
            return await _productRepository.AnyAsync(x => x.Id == productId);
        }
    }
}
