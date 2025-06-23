using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers
{
    public interface IFileProvider
    {
        Task <Result<string, Error>> UploadFile(
            FileData fileData, CancellationToken cancellationToken = default);
    }
}
