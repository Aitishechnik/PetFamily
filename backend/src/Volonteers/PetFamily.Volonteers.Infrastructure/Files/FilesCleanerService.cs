using Microsoft.Extensions.Logging;
using PetFamily.Core.FileManagement.Providers;
using PetFamily.Core.FileManagment.Files;
using PetFamily.Core.Messaging;

namespace PetFamily.Volonteers.Infrastructure.Files
{
    public partial class FilesCleanerBackgroundService
    {
        public class FilesCleanerService : IFilesCleanerService
        {
            private readonly ILogger _logger;
            private readonly IFileProvider _fileProvider;
            private readonly IMessageQueue<IEnumerable<FileInfoPath>> _messageQueue;
            public FilesCleanerService(
                IFileProvider fileProvider,
                ILogger<FilesCleanerService> logger,
                IMessageQueue<IEnumerable<FileInfoPath>> messageQueue)
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
