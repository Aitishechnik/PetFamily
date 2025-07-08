using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using Minio;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;
using Microsoft.Extensions.Configuration;
using PetFamily.Infrastructure.Options;
using PetFamily.Contracts;

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
            catch (Exception e)
            {
                _logger.LogError(e, "Fail to delete file in minio");
                return Error.Failure("file.delete", "Fail to delete file in minio");
            }
        }

        public async Task<Result<string, Error>> GetPresignedFile(string FileName, CancellationToken cancellationToken = default)
        {
            var bucketName = GetBucketNameFromConfig();

            var bucketExists = await IsBucketExist(bucketName, cancellationToken);
            if (bucketExists == false)
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

        public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
            IReadOnlyList<FileDTO> filesDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketName = GetBucketNameFromConfig();
                await CreateBucketIfNotExists(bucketName, cancellationToken);
                var semaphoreSlim = new SemaphoreSlim(GetMaxWriteConcurrencyFromConfig());

                var filesList = filesDTO.ToList();

                var tasks = filesList.Select(async dto =>
                {
                    return await PutObject(dto, semaphoreSlim, cancellationToken);
                });

                var pathsResult = await Task.WhenAll(tasks);

                var failResult = pathsResult.Where(p => p.IsFailure);
                if(failResult.Count() > 0)
                    return failResult.First().Error;

                return pathsResult.Select(p => p.Value).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Fail to upload files in minio");
                return Error.Failure("file.upload", "Fail to upload files in minio");
            }
        }

        private async Task CreateBucketIfNotExists(
            string bucketName, 
            CancellationToken cancellationToken)
        {
            if (!await IsBucketExist(bucketName, cancellationToken))
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
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

        private int GetMaxWriteConcurrencyFromConfig()
        {
            return _configuration.GetSection(MinioBucketOptions.MINIO_BUCKETS)
                    .Get<MinioBucketOptions>()?.MaxWriteConcurrency
                    ?? throw new ApplicationException("Max write concurrency is missing");
        }

        private async Task<Result<FilePath, Error>> PutObject(
        FileDTO fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var backetName = GetBucketNameFromConfig();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(backetName)
                .WithStreamData(fileData.Content)
                .WithObjectSize(fileData.Content.Length)
                .WithObject(fileData.FileName);

            try
            {
                await _minioClient
                    .PutObjectAsync(putObjectArgs, cancellationToken);

                return FilePath.Create(fileData.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileData.FileName,
                    backetName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
