using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Infrastructure.Options;

namespace Scratch.Infrastructure.Services
{
    public class ObjectStorageService : IObjectStorageService
    {
        private readonly S3Options _options;
        private readonly IAmazonS3 _s3Client;

        private readonly string _savePath = @"C:\\Users\\Admin\\Desktop\\_\\playground\\scratch-project\\test-storage";
        private readonly string _s3Path = @"https://pub-962bffdbd95945c6a12e8c14089413e4.r2.dev/";

        public ObjectStorageService(IOptions<S3Options> s3Options)
        {
            _options = s3Options.Value;

            var credentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
            var config = new AmazonS3Config
            {
                ServiceURL = _options.URL,
                
            };
            _s3Client = new AmazonS3Client(credentials, config);
        }

        public async Task<string> Save(string name, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = _options.Bucket,
                Key = name,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true,
                DisableDefaultChecksumValidation = true,
            };

            await _s3Client.PutObjectAsync(request);

            return _s3Path + name;
        }

        public string GetPath(string name)
        {
            return _s3Path + name;
        }

        public bool TryGetPreSignedUrl(string name, string contentType, string contentLength, out string preSignedUrl)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _options.Bucket,
                    Key = name,
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    Verb = HttpVerb.PUT,
                    ContentType = contentType,
                };

                request.Headers["Content-Length"] = contentLength;

                preSignedUrl = _s3Client.GetPreSignedURL(request);
                return true;
            }
            catch (AmazonS3Exception)
            {
                preSignedUrl = string.Empty;
                return false;
            }
        }
    }
}
