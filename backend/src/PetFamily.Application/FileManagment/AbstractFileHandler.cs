using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Providers;

namespace PetFamily.Application.FileManagement
{
    public abstract class AbstractFileHandler<T> where T : class
    {
        protected readonly IFileProvider _fileProvider;
        protected readonly ILogger<T> _logger;

        public AbstractFileHandler(IFileProvider fileProvider,
            ILogger<T> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

    }
}
