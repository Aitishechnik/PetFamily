using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Presign
{
    public class GetPresignedHandler : AbstractFileHandler<GetPresignedHandler>
    {
        public GetPresignedHandler(IFileProvider fileProvider, ILogger<GetPresignedHandler> logger) 
            : base(fileProvider, logger) { }

        public async Task<Result<string, Error>> Handle(
            GetPresignedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _fileProvider.GetPresignedFile(request.ObjectName, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError("Failed to get presigned URL for file {FileName}: {ErrorMessage}", request.ObjectName, result.Error.Message);
                return result.Error;
            }
            return result;
        }
    }
}
