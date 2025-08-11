using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Core.Dtos;
using PetFamily.Core.FileManagement.Providers;
using PetFamily.Core.FileManagment.Files;
using PetFamily.Core.Options;
using PetFamily.SharedKernal;

namespace PetFamily.Volonteers.Infrastructure.Providers
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

        public async Task<UnitResult<Error>> DeleteFiles(
            IEnumerable<FileInfoPath> filesInfo, CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketName = filesInfo.FirstOrDefault()?.Bucket
                    ?? throw new Exception("empty filesInfo collection");

                var semaphoreSlim = new SemaphoreSlim(GetMaxDeleteConcurrencyFromConfig());
                var bucketExists = await IsBucketExist(bucketName!, cancellationToken);
                if (!bucketExists)
                {
                    return Error.Failure("file.delete", "Bucket does not exist");
                }

                var tasksResult = filesInfo.Select(async fileInfo =>
                {
                    return await RemoveObject(
                        fileInfo.Bucket,
                        fileInfo.FilePath.Path,
                        semaphoreSlim, cancellationToken);
                });

                var tasksResults = await Task.WhenAll(tasksResult);

                var result = tasksResult.FirstOrDefault(t => t.Result.IsFailure);
                if (result != null)
                    return result.Result.Error;

                return Result.Success<Error>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Fail to delete file in minio");
                return Error.Failure("file.delete", "Fail to delete file in minio");
            }
        }

        public async Task<Result<string, Error>> GetPresignedFile(
            string FileName, CancellationToken cancellationToken = default)
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
            IReadOnlyList<FileDto> filesDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketName = GetBucketNameFromConfig();
                await CreateBucketIfNotExists(bucketName, cancellationToken);
                var semaphoreSlim = new SemaphoreSlim(GetMaxWriteConcurrencyFromConfig());

                var filesList = filesDTO.ToList();

                var tasks = filesList.Select(async dto =>
                {
                    return await PutObject(
                        bucketName, dto, semaphoreSlim, cancellationToken);
                });

                var pathsResult = await Task.WhenAll(tasks);

                var failResult = pathsResult.Where(p => p.IsFailure);
                if (failResult.Count() > 0)
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

        private async Task<bool> IsBucketExist(
            string bucketName, CancellationToken cancellationToken)
        {
            var backetExistsArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

            return await _minioClient.BucketExistsAsync(
                backetExistsArgs, cancellationToken);
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

        private int GetMaxDeleteConcurrencyFromConfig()
        {
            return _configuration.GetSection(MinioBucketOptions.MINIO_BUCKETS)
                    .Get<MinioBucketOptions>()?.MaxDeleteConcurrency
                    ?? throw new ApplicationException("Max delete concurrency is missing");
        }

        private async Task<UnitResult<Error>> RemoveObject(
            string bucketName,
            string filePath,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken
            )
        {
            await semaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath);
                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
                return Result.Success<Error>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Fail to remove file in minio with path {path} in bucket {bucket}",
                    filePath,
                    bucketName);
                return Error.Failure("file.remove", "Fail to remove file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task<Result<FilePath, Error>> PutObject(
        string bucketName,
        FileDto fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
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
                _logger.LogError(
                    ex,
                    "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileData.FileName,
                    bucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
