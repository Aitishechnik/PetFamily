using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Providers;

namespace PetFamily.Infrastructure.BackgroundServices
{
    public partial class FilesCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<FilesCleanerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FilesCleanerBackgroundService(
            ILogger<FilesCleanerBackgroundService> logger,
            IServiceScopeFactory scopeFactory)

        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FilesCleanerBackgroundService is started");

            await using var scope = _scopeFactory.CreateAsyncScope();

            var fileCleanerService = scope.ServiceProvider.GetRequiredService<IFilesCleanerService>();

            while (!stoppingToken.IsCancellationRequested)
            {
                await fileCleanerService.Process(stoppingToken);
            }

            await Task.CompletedTask;
        }
    }
}
