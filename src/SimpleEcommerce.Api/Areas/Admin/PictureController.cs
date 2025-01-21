using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Api.Dtos.Media;
using SimpleEcommerce.Api.Models.Media;
using SimpleEcommerce.Api.Services.Media;
namespace SimpleEcommerce.Api.Areas.Admin
{

    [Route("api/medias")]
    [Authorize]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<PictureDto> Upload([FromForm] PictureModel model)
        {
            var response = await _pictureService.CreatePictureAsync(model);

            return response;
        }
    }
}
