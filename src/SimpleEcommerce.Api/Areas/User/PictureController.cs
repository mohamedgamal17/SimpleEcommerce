using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos.Media;
using SimpleEcommerce.Api.Models.Media;
using SimpleEcommerce.Api.Services.Media;
namespace SimpleEcommerce.Api.Areas.User
{
    [Route("api/pictures")]
    [Authorize]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private IPictureService _pictureService;
        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [Route("upload")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PictureDto))]
        public async Task<PictureDto> Upload([FromForm] PictureModel model)
        {
            var response = await _pictureService.CreateUserAvatarAsync(model);

            return response;
        }
    }
}
