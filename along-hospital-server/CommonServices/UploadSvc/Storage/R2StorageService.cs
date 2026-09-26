using Amazon.S3;
using Amazon.S3.Model;
using SharedLibrary.Commons;

namespace UploadSvc.Storage
{
    public class R2StorageService(IAmazonS3 s3Client, AppConfiguration configuration) : IStorageService
    {
        private readonly IAmazonS3 _s3Client = s3Client;
        private readonly AppConfiguration _configuration = configuration;

        public async Task UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = _configuration.R2Config.Bucket,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead,
                UseChunkEncoding = false,
                AutoResetStreamPosition = true,
                AutoCloseStream = false
            };

            await _s3Client.PutObjectAsync(request);
        }

        public async Task DeleteAsync(string fileName)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _configuration.R2Config.Bucket,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(request);
        }
    }
}
