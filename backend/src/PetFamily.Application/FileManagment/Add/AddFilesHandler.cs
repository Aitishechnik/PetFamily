using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Add
{
    public class AddFilesHandler
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
            AddFilesRequest request, 
            CancellationToken cancellationToken = default)
        {
            return await _fileProvider
                .UploadFiles(request.FilesDTO, cancellationToken);
        }
    }
}
