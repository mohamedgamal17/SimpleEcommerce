using FluentValidation;
using SimpleEcommerce.Api.Domain.Users;
using SimpleEcommerce.Api.EntityFramework;

namespace SimpleEcommerce.Api.Models.Sales
{
    public class OrderModel
    {
        public string UserId { get; set; }
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
    }

    public class OrderModelValidator : AbstractValidator<OrderModel>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<User> _userRepository;
        public OrderModelValidator(IServiceProvider serviceProvider, IRepository<User> userRepository)
        {
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;

            RuleFor(x => x.UserId)
                .MustAsync(CheckUserId)
                .WithMessage("User id should be valid");

            RuleFor(x => x.Items)
                .NotNull()
                .Must(x => x.Count > 0)
                .WithMessage("Order should contain at least one item");

            RuleForEach(x => x.Items)
                .SetValidator(_serviceProvider.GetRequiredService<OrderItemModelValidator>());
          
        }

        private async Task<bool> CheckUserId(string userId, CancellationToken cancellationToken)
        {
            return await _userRepository.AnyAsync(x => x.Id == userId);
        }
    }
}
