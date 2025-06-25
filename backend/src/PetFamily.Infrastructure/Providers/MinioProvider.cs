using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using Minio;
using PetFamily.Application.TestMinio.Providers;
using PetFamily.Domain.Shared;
using Microsoft.Extensions.Configuration;
using PetFamily.Infrastructure.Options;

namespace PetFamily.Infrastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;
        private readonly IConfiguration _configuration;
        public MinioProvider(
            IMinioClient minioClient, 
            ILogger<MinioProvider> logger,
            IConfiguration configuration)
        {
            _minioClient = minioClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Result<string, Error>> DeleteFile(string objectName, CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketName = GetBucketNameFromConfig();

                var bucketExists = await IsBucketExist(bucketName, cancellationToken);
                if (!bucketExists)
                {
                    return Error.Failure("file.delete", "Bucket does not exist");
                }
                var deleteObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);

                await _minioClient.RemoveObjectAsync(deleteObjectArgs, cancellationToken);

                return objectName;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Fail to delete file in minio");
                return Error.Failure("file.delete", "Fail to delete file in minio");
            }
        }

        public async Task<Result<string, Error>> GetPresignedFile(string FileName, CancellationToken cancellationToken = default)
        {
            var bucketName = GetBucketNameFromConfig();

            var bucketExists = await IsBucketExist(bucketName, cancellationToken);
            if(bucketExists == false)
            {
                return Error.Failure("file.presigned", "Bucket does not exist");
            }

            try
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(FileName)
                    .WithExpiry(MinioBucketOptions.EXPIRY_TIME_IN_SECONDS);
                var url = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
                return url;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Fail to get presigned file in minio");
                return Error.Failure("file.presigned", "Fail to get presigned file in minio");
            }
        }

        public async Task<Result<string, Error>> UploadFile(Stream stream, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var bucketName = GetBucketNameFromConfig();

                var bucketExists = await IsBucketExist(bucketName, cancellationToken);
                if(bucketExists == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                var path = Guid.NewGuid();

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithObject($"{path}/{fileName}");

                var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                return result.ObjectName;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Fail to upload file in minio");
                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
        }

        private async Task<bool> IsBucketExist(string bucketName, CancellationToken cancellationToken)
        {
            var backetExistsArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

            return await _minioClient.BucketExistsAsync(backetExistsArgs, cancellationToken);
        }

        private string GetBucketNameFromConfig()
        {
            return _configuration.GetSection(MinioBucketOptions.MINIO_BUCKETS)
                    .Get<MinioBucketOptions>()?.Photos
                    ?? throw new ApplicationException("Bucket info is missing");
        }
    }
}
