using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Dtos.Media;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Factories.Media;
using SimpleEcommerce.Api.Models.Media;
using SimpleEcommerce.Api.Security;
using SimpleEcommerce.Api.Services.Storage;

namespace SimpleEcommerce.Api.Services.Media
{
    public class PictureService : IPictureService
    {
        private readonly IRepository<Picture> _pictureRepository;
        private readonly S3StorageService _s3StorageService;
        private readonly ICurrentUser _currentUser;
        private readonly MediaResponseFactory _mediaResponseFactory;

        public PictureService(IRepository<Picture> pictureRepository, S3StorageService s3StorageService, ICurrentUser currentUser, MediaResponseFactory mediaResponseFactory)
        {
            _pictureRepository = pictureRepository;
            _s3StorageService = s3StorageService;
            _currentUser = currentUser;
            _mediaResponseFactory = mediaResponseFactory;
        }

        public async Task<PictureDto> CreateUserAvatarAsync(PictureModel model, CancellationToken cancellationToken = default)
        {
            string currentUserId = _currentUser.Id!;

            string fileName = $"{model.Image.FileName.Split(".")[0]}_{DateTime.Now.Ticks}.{model.Image.FileName.Split(".")[1]}";

            string s3Key = $"users/{currentUserId}/{fileName}";

            await _s3StorageService.SaveObjectAsync(s3Key, model.Image.OpenReadStream(), model.Image.ContentType);

            var picture = new Picture
            {
                MimeType = model.Image.ContentType,
                AltAttribute = model.AltAttribute ?? currentUserId,
                PictureType = PictureType.Avatar,
                S3Key = s3Key
            };

            await _pictureRepository.InsertAsync(picture);

            var response = await _mediaResponseFactory.PrepareDto(picture);

            return response;
        }
        public async Task<PictureDto> CreatePictureAsync(PictureModel model, CancellationToken cancellationToken = default)
        {
            string fileName = $"{model.Image.FileName.Split(".")[0]}_{DateTime.Now.Ticks}.{model.Image.FileName.Split(".")[1]}";

            string s3Key = $"thumbs/{fileName}";

            await _s3StorageService.SaveObjectAsync(s3Key, model.Image.OpenReadStream(), model.Image.ContentType);

            var picture = new Picture
            {
                MimeType = model.Image.ContentType,
                AltAttribute = model.AltAttribute ?? fileName,
                PictureType = PictureType.Entity,
                S3Key = s3Key
            };

            await _pictureRepository.InsertAsync(picture);

            var response = await _mediaResponseFactory.PrepareDto(picture);

            return response;
        }

        
    }
}
