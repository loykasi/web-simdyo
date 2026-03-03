using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces.Services;
using Hangfire;
using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class ObjectStorageService : IObjectStorageService
    {
        private readonly S3Options _options;
        private readonly IAmazonS3 _s3Client;

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

            return _options.PublicURL + name;
        }

        public void DeleteJobs(IEnumerable<string> names)
        {
            BackgroundJob.Enqueue(() => Delete(names));
        }

        public async Task<bool> Delete(IEnumerable<string> names)
        {
            var request = new DeleteObjectsRequest
            {
                BucketName = _options.Bucket,
            };
            foreach (var item in names)
            {
                request.AddKey(item);
            }
            try
            {
                await _s3Client.DeleteObjectsAsync(request);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetPath(string name)
        {
            return _options.PublicURL + name;
        }

        public bool TryGetPreSignedUrl(string name, string contentType, long contentLength, out string preSignedUrl)
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

                request.Headers["Content-Length"] = contentLength.ToString();

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
