namespace SimpleEcommerce.Api.Services.Jwt
{
    public class JwtToken
    {
        public string Token { get; set; }
        public long NotBefore { get; set; }
        public long ExpiresAt { get; set; }
    }
}
