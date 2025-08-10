using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.FileManagement.Providers;
using PetFamily.SharedKernal;

namespace PetFamily.Core.FileManagement.Delete
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
