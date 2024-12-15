using AutoMapper;
using SimpleEcommerce.Api.Domain.Media;
using SimpleEcommerce.Api.Services;

namespace SimpleEcommerce.Api.Dtos.Media
{
    public class MediaProfileMapping : Profile
    {
        public MediaProfileMapping()
        {
            CreateMap<Picture, PictureDto>()
                .ForMember(x => x.Url, opt => opt.MapFrom<MediaUrlValueResolver>());
        }
    }


    public class MediaUrlValueResolver : IValueResolver<Picture, PictureDto, string>
    {
        private readonly S3StorageService _s3StoragetService;
        public MediaUrlValueResolver(S3StorageService s3StoragetService)
        {
            _s3StoragetService = s3StoragetService;
        }
        public  string Resolve(Picture source, PictureDto destination, string destMember, ResolutionContext context)
        {
            return _s3StoragetService.GeneratePublicUrl(source.S3Key); 
        }
    }
}
