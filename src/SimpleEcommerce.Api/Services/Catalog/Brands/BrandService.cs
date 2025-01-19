using SimpleEcommerce.Api.Dtos.Catalog;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Models.Catalog;
using SimpleEcommerce.Api.Models.Common;
using SimpleEcommerce.Api.Domain.Catalog;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Factories.Catalog;
using SimpleEcommerce.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Exceptions;
namespace SimpleEcommerce.Api.Services.Catalog.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly BrandResponseFactory _brandResposneFactory;

        public BrandService(IRepository<Brand> brandRepository, BrandResponseFactory brandResposneFactory)
        {
            _brandRepository = brandRepository;
            _brandResposneFactory = brandResposneFactory;
        }

        public async Task<BrandDto> CreateAsync(BrandModel model, CancellationToken cancellationToken = default)
        {
            var nameExist = await _brandRepository.AnyAsync(x => x.Name == model.Name);

            if (nameExist)
            {
                throw new BusinessLogicException($"Brand name : ${model.Name} , is already exist choose another name");
            }

            var brand = new Brand
            {
                Name = model.Name,
                Description = model.Description
            };
            await _brandRepository.InsertAsync(brand);

            var response = await _brandResposneFactory.PrepareDto(brand);

            return response;
        }

        public async Task<BrandDto> UpdateAsync(string id, BrandModel model, CancellationToken cancellationToken = default)
        {
            var brand = await _brandRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (brand == null)
            {
                throw new EntityNotFoundException(typeof(Brand), id);
            }

            var nameExist = await _brandRepository.AnyAsync(x => x.Name == model.Name && x.Id != id);

            if (nameExist)
            {
                throw new BusinessLogicException($"Brand name : ${model.Name} , is already exist choose another name");
            }

            brand.Name = model.Name;
            brand.Description = model.Description;

            await _brandRepository.UpdateAsync(brand);

            var response = await _brandResposneFactory.PrepareDto(brand);

            return response;
        }

        public async Task<PagedDto<BrandDto>> ListPagedAsync(PagingModel model, CancellationToken cancellationToken = default)
        {
            var result = await _brandRepository.AsQuerable()
              .ToPaged(model.Skip, model.Limit);

            var response = await _brandResposneFactory.PreparePagedDto(result);

            return response;
        }

        public async Task<BrandDto> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _brandRepository.AsQuerable();

            var result = await query.SingleOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new EntityNotFoundException(typeof(Brand), id);
            }

            var response = await _brandResposneFactory.PrepareDto(result);

            return response;
        }
    }
}
