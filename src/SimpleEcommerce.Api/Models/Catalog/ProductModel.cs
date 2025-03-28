﻿using FluentValidation;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.EntityFramework;

namespace SimpleEcommerce.Api.Models.Catalog
{
    public class ProductModel
    {
        public string  Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Brands { get; set; }

        public List<ProductPictureModel> Pictures { get; set; }

    }

    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepoistory;
        private readonly IServiceProvider _serviceProvider;
        public ProductModelValidator(IRepository<Category> categoryRepository, IRepository<Brand> brandRepoistory, IServiceProvider serviceProvider)
        {
            _categoryRepository = categoryRepository;
            _brandRepoistory = brandRepoistory;
            _serviceProvider = serviceProvider;

            RuleFor(x => x.Name)
                .MinimumLength(3)
                .MaximumLength(600)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(1500)
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThanOrEqualTo(int.MaxValue);

            RuleForEach(x => x.Categories)
                .MaximumLength(450)
                .MustAsync(CheckCategoryExistance)
                .WithMessage((_, categoryId) => $"Category with id : (${categoryId}) , is not exist")
                .When(x => x.Categories != null);

            RuleFor(x => x.Categories)
                .Must(x => x?.Count <= 10)
                .WithMessage("Product cannot be assigned to more than 10 categories")
                .When(x => x.Categories != null);

            RuleForEach(x => x.Brands)
                .MaximumLength(450)
                .MustAsync(CheckBrandExistance)
                .WithMessage((_, brandId) => $"Brand with id : (${brandId}) , is not exist")
                .When(x => x.Brands != null);

            RuleFor(x => x.Brands)
                .Must(x => x?.Count <= 10)
                .WithMessage("Product cannot be assigned to more than 10 brands")
                .When(x => x.Brands != null);

            RuleFor(x => x.Pictures)
                .Must(x => x.Count <= 10)
                .WithMessage("Product cannot has more than 10 pictures.")
                .When(x => x.Pictures != null);

            RuleForEach(x => x.Pictures)
                .SetValidator(_serviceProvider.GetRequiredService<ProductPictureModelValidator>())
                .When(x => x.Pictures != null);
        }

        private async Task<bool> CheckCategoryExistance(string categoryId , CancellationToken cancellationToken = default)
        {
            return await _categoryRepository.AnyAsync(x => x.Id == categoryId);
        }

        private async Task<bool> CheckBrandExistance(string brandId , CancellationToken cancellationToken = default)
        {
            return await _brandRepoistory.AnyAsync(x => x.Id == brandId);
        }

    }
}
