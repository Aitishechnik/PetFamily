using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;
        public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken)
        {
            try
            {
                var backetExistsArgs = new BucketExistsArgs()
                    .WithBucket("photos");

                var bucketExists = await _minioClient.BucketExistsAsync(backetExistsArgs, cancellationToken);
                if(bucketExists == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket("photos");
                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                var path = Guid.NewGuid();

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket("photos")
                    .WithStreamData(fileData.Stream)
                    .WithObjectSize(fileData.Stream.Length)
                    .WithObject($"{path}/{fileData.ObjectName}");

                var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                return result.ObjectName;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Fail to upload file in minio");
                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
        }
    }
}
