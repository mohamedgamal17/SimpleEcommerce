using FluentValidation;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.EntityFramework;

namespace SimpleEcommerce.Api.Models.Cart
{
    public class BasketItemModel
    {
        public int ProductId { get; set; }

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
                .MustAsync(CheckProductExist)
                .WithMessage((_, productId) => $"Product with id : ({productId}) , is not exist");

        }

        private async Task<bool> CheckProductExist(int productId , CancellationToken cancellationToken = default)
        {
            return await _productRepository.AnyAsync(x => x.Id == productId);
        }
    }
}
