using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Delete
{
    public class DeleteFileHandler : AbstractFileHandler<DeleteFileHandler>
    {
        public DeleteFileHandler(IFileProvider fileProvider, ILogger<DeleteFileHandler> logger)
            : base(fileProvider, logger) { }

        public async Task<Result<string, Error>> Handle(
            DeleteFileRequest request, CancellationToken cancellationToken = default)
        {

            var result = await _fileProvider.DeleteFile(request.ObjectName, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError("Failed to delete file {FileName}: {ErrorMessage}", request.ObjectName, result.Error.Message);
                return result.Error;
            }

            return result;
        }
    }
}
