using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Presign
{
    public class GetPresignedHandler : IFileHandler
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<GetPresignedHandler> _logger;
        public GetPresignedHandler(
            IFileProvider fileProvider, 
            ILogger<GetPresignedHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

        public async Task<Result<string, Error>> Handle(
            GetPresignedCommand command, 
            CancellationToken cancellationToken = default)
        {
            var result = await _fileProvider
                .GetPresignedFile(command.ObjectName, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError(
                    "Failed to get presigned URL for file {FileName}: {ErrorMessage}", 
                    command.ObjectName, 
                    result.Error.Message);
                return result.Error;
            }
            return result;
        }
    }
}
