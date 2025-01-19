using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Exceptions;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Factories.Catalog;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
namespace SimpleEcommerce.Api.Services.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly CategoryResponseFactory _categoryResposneFactory;

        public CategoryService(IRepository<Category> categoryRepository, CategoryResponseFactory categoryResposneFactory)
        {
            _categoryRepository = categoryRepository;
            _categoryResposneFactory = categoryResposneFactory;
        }

        public async Task<CategoryDto> CreateAsync(CategoryModel model, CancellationToken cancellationToken = default)
        {
            var nameExist = await _categoryRepository.AnyAsync(x => x.Name == model.Name);

            if (nameExist)
            {
                throw new BusinessLogicException($"Category name : ${model.Name} , is already exist choose another name");
            }

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            await _categoryRepository.InsertAsync(category);

            var response = await _categoryResposneFactory.PrepareDto(category);

            return response;
        }

        public async Task<CategoryDto> UpdateAsync(string id, CategoryModel model, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                throw new EntityNotFoundException(typeof(Category), id);
            }

            var nameExist = await _categoryRepository.AnyAsync(x => x.Name == model.Name && x.Id != id);

            if (nameExist)
            {
                throw new BusinessLogicException($"Category name : ${model.Name} , is already exist choose another name");
            }

            category.Name = model.Name;
            category.Description = model.Description;

            await _categoryRepository.UpdateAsync(category);

            var response = await _categoryResposneFactory.PrepareDto(category);

            return response;
        }

        public async Task<CategoryDto> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _categoryRepository.AsQuerable()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Category), id);
            }

            var resposne = await _categoryResposneFactory.PrepareDto(result);

            return resposne;
        }

        public async Task<PagedDto<CategoryDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default)
        {
            var result = await _categoryRepository.AsQuerable()
                .ToPaged(model.Skip, model.Limit);

            var resposne = await _categoryResposneFactory.PreparePagedDto(result);

            return resposne;
        }  
    }
}
