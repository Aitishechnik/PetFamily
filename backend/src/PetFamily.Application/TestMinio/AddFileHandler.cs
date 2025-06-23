using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.TestMinio
{
    public class AddFileHandler
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<AddFileHandler> _logger;

        public AddFileHandler(IFileProvider fileProvider,
            ILogger<AddFileHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

        public async Task<Result<string, Error>> Handle(
            FileData fileData, CancellationToken cancellationToken = default)
        {
            return await _fileProvider.UploadFile(fileData, cancellationToken);
        }
    }
}
