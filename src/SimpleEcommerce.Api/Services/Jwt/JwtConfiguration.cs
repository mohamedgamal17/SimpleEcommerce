namespace SimpleEcommerce.Api.Services.Jwt
{
    public class JwtConfiguration
    {
        public const string CONFIG_KEY = "Jwt";

        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }

    }
}
