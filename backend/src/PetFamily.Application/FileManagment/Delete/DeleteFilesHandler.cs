using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Add;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Delete
{
    public class DeleteFilesHandler
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
            DeleteFilesRequest request, 
            CancellationToken cancellationToken = default)
        {
            var result = await _fileProvider
                .DeleteFiles(request.ObjectsNames, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError(
                    "Failed to delete file {FileName}: {ErrorMessage}"
                    , request.ObjectsNames, result.Error.Message);
                return result.Error;
            }

            return result;
        }
    }
}
