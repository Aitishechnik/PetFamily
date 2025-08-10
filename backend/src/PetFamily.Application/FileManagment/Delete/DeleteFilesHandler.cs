using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Delete
{
    public class DeleteFilesHandler : IFileHandler
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<DeleteFilesHandler> _logger;
        public DeleteFilesHandler(
            IFileProvider fileProvider,
            ILogger<DeleteFilesHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

        public async Task<UnitResult<Error>> Handle(
            DeleteFilesCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _fileProvider
                .DeleteFiles(command.filesInfo, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError(
                    "Failed to delete file {FileName}: {ErrorMessage}"
                    , command.filesInfo, result.Error.Message);
                return result.Error;
            }

            return result;
        }
    }
}
