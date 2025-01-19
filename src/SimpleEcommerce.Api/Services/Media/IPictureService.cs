using SimpleEcommerce.Api.Dtos.Media;
using SimpleEcommerce.Api.Models.Media;
namespace SimpleEcommerce.Api.Services.Media
{
    public interface IPictureService
    {
        Task<PictureDto> CreateUserAvatarAsync(PictureModel model, CancellationToken cancellationToken = default);

        Task<PictureDto> CreatePictureAsync(PictureModel model, CancellationToken cancellationToken = default);
    }
}
