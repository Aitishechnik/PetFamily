using Microsoft.AspNetCore.Http;

namespace PetFamily.Core.Processors
{
    public class FormFileProcessor : IAsyncDisposable
    {
        private readonly List<Stream> _fileDtos = [];

        public List<Stream> Process(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                _fileDtos.Add(stream);
            }

            return _fileDtos;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var file in _fileDtos)
            {
                await file.DisposeAsync();
            }
        }
    }
}
