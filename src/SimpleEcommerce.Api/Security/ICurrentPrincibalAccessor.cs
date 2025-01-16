using System.Security.Claims;

namespace SimpleEcommerce.Api.Security
{
    public interface ICurrentPrincibalAccessor
    {
        ClaimsPrincipal? Principal { get; }
    }



}
