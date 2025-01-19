using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Dtos;
using SimpleEcommerce.Api.Dtos.Media;
using SimpleEcommerce.Api.Dtos.Sales;
using SimpleEcommerce.Api.EntityFramework;
using SimpleEcommerce.Api.Models.Media;
using SimpleEcommerce.Api.Security;
using SimpleEcommerce.Api.Services;
namespace SimpleEcommerce.Api.Areas.User
{

    [Route("api/pictures")]
    [Authorize]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IRepository<Picture> _pictureRepository;
        private readonly S3StorageService _s3StorageService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public PictureController(IRepository<Picture> pictureRepository, S3StorageService s3StorageService, IMapper mapper, ICurrentUser currentUser)
        {
            _pictureRepository = pictureRepository;
            _s3StorageService = s3StorageService;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        [Route("upload")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PictureDto))]
        public async Task<PictureDto> Upload([FromForm] PictureModel model)
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

            return _mapper.Map<Picture, PictureDto>(picture);
        }
    }
}
