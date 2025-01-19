namespace SimpleEcommerce.Api.Services.Storage
{
    public interface IS3StorageService
    {
        string GeneratePublicUrl(string key);
        Task<string> SaveObjectAsync(string key, Stream content, string contentType);
    }
}