using Minio;
using Minio.DataModel.Args;
using System.Text;

namespace SimpleEcommerce.Api.Services
{
    public class S3StorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly S3StorageConfiguration _s3StorageConfiguration;

        public S3StorageService(S3StorageConfiguration s3StorageConfiguration)
        {
        
            _s3StorageConfiguration = s3StorageConfiguration;
            _minioClient = GenerateMinioClient(s3StorageConfiguration);
        }


        public async Task<string> SaveObjectAsync(string key , Stream content , string contentType)
        {
            await CreateBucketIfNotExist();
           
            var args = new PutObjectArgs()
                .WithBucket(_s3StorageConfiguration.BucketName)
                .WithFileName(key)
                .WithContentType(contentType)
                .WithStreamData(content)
                .WithObjectSize(content.Length);


            var response = await _minioClient.PutObjectAsync(args);

            return response.ObjectName;
        }


        public string GeneratePublicUrl(string key)
        {
            StringBuilder urlBuilder = new StringBuilder();

            urlBuilder.Append(_s3StorageConfiguration.UseSSL ? "https://" : "http://");

            urlBuilder.Append($"{_s3StorageConfiguration.EndPoint}/{_s3StorageConfiguration.BucketName}/{key}");

            return urlBuilder.ToString();
        }

        private async Task CreateBucketIfNotExist()
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(_s3StorageConfiguration.BucketName);

            var isExist = await _minioClient.BucketExistsAsync(bucketExistArgs);

            if (!isExist)
            {
                var maketBucketArgs = new MakeBucketArgs()
                    .WithBucket(_s3StorageConfiguration.BucketName);

                await _minioClient.MakeBucketAsync(maketBucketArgs);
            }
        }

        private IMinioClient GenerateMinioClient(S3StorageConfiguration configuration)
        {
            var client = new MinioClient()
                .WithEndpoint(configuration.EndPoint)
                .WithCredentials(configuration.AccessKey, configuration.SecretKey);

            if (configuration.UseSSL)
            {
                client = client.WithSSL();
            }

            return client.Build();
        }
    }


    public class S3StorageConfiguration
    {
        public const string CONFIG_KEY = "S3Storage";

        public string EndPoint { get; set; }
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set;}
        public bool UseSSL { get; set; }

    }
}
