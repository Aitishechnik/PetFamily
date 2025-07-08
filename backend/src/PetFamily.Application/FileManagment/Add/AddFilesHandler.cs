using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Contracts;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Add
{
    public class AddFilesHandler : AbstractFileHandler<AddFilesHandler>
    {
        public AddFilesHandler(IFileProvider fileProvider, ILogger<AddFilesHandler> logger)
            : base(fileProvider, logger) { }
        public async Task<Result<IReadOnlyList<FilePath>, Error>> Handle(
            AddFilesRequest request, CancellationToken cancellationToken = default)
        {
            return await _fileProvider.UploadFiles(request.FilesDTO, cancellationToken);
        }
    }
}
