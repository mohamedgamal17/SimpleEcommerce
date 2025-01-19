using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Dtos.Media;
using SimpleEcommerce.Api.Services;

namespace SimpleEcommerce.Api.Factories.Media
{
    public class MediaResponseFactory : ResponseFactory<Picture, PictureDto>
    {
        private readonly IS3StorageService _s3StorageService;

        public MediaResponseFactory(IS3StorageService s3StorageService)
        {
            _s3StorageService = s3StorageService;
        }

        public override Task<PictureDto> PrepareDto(Picture data)
        {
            var dto = new PictureDto
            {
                Id = data.Id,
                MimeType = data.MimeType,
                PictureType = data.PictureType,
                AltAttribute = data.AltAttribute,
                Url = _s3StorageService.GeneratePublicUrl(data.S3Key)
            };

            return Task.FromResult(dto);
        }
    }
}
