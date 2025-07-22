using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Add
{
    public class AddFilesHandler : IFileHandler
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<AddFilesHandler> _logger;
        public AddFilesHandler(
            IFileProvider fileProvider, 
            ILogger<AddFilesHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }
        public async Task<Result<IReadOnlyList<FilePath>, Error>> Handle(
            AddFilesCommand command, 
            CancellationToken cancellationToken = default)
        {
            return await _fileProvider
                .UploadFiles(command.FilesDTO, cancellationToken);
        }
    }
}
