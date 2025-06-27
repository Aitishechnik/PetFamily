using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.TestMinio.Providers
{
    public interface IFileProvider
    {
        Task <Result<string, Error>> UploadFile(
            Stream stream, string fileData, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> DeleteFile(string objectName, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetPresignedFile(string FileName, CancellationToken cancellationToken = default);
    }
}
