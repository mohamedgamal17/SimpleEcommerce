using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Dtos.Media;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Models.Media;
using SimpleEcommerce.Api.Services;
namespace SimpleEcommerce.Api.Areas.Admin
{

    [Route("api/medias")]
    [Authorize]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IRepository<Picture> _pictureRepository;
        private readonly S3StorageService _s3StorageService;
        private readonly IMapper _mapper;
        public PictureController(IRepository<Picture> pictureRepository, S3StorageService s3StorageService, IMapper mapper)
        {
            _pictureRepository = pictureRepository;
            _s3StorageService = s3StorageService;
            _mapper = mapper;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<PictureDto> Upload([FromForm] PictureModel model)
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

            return _mapper.Map<Picture, PictureDto>(picture);
        }
    }
}
