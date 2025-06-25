using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.TestMinio.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.TestMinio.Add
{
    public class AddFileHandler : AbstractFileHandler<AddFileHandler>
    {
        public AddFileHandler(IFileProvider fileProvider, ILogger<AddFileHandler> logger)
            : base(fileProvider, logger) { }
        public async Task<Result<string, Error>> Handle(
            AddFileRequest request, CancellationToken cancellationToken = default)
        {
            return await _fileProvider.UploadFile(request.Stream, request.FileName, cancellationToken);
        }
    }
}
