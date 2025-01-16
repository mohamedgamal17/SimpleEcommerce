using System.Security.Claims;

namespace SimpleEcommerce.Api.Security
{
    public class HttpContextCurrentPrincibalAccessor : ICurrentPrincibalAccessor
    {
        public ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext.User;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentPrincibalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }



}
