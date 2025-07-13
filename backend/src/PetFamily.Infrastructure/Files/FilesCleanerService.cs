using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Application.Messaging;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.Infrastructure.Files
{
    public partial class FilesCleanerBackgroundService
    {
        public class FilesCleanerService : IFilesCleanerService
        {
            private readonly ILogger _logger;
            private readonly IFileProvider _fileProvider;
            private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
            public FilesCleanerService(
                IFileProvider fileProvider,
                ILogger<FilesCleanerService> logger,
                IMessageQueue<IEnumerable<FileInfo>> messageQueue)
            {
                _logger = logger;
                _fileProvider = fileProvider;
                _messageQueue = messageQueue;
            }
            public async Task Process(CancellationToken cancelationToken = default)
            {
                _logger.LogInformation("FilesCleanerService - start Reading queue");
                var filesInfo = await _messageQueue.ReadAsync(cancelationToken);

                await _fileProvider.DeleteFiles(filesInfo, cancelationToken);
            }
        }
    }
}
