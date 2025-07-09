using CSharpFunctionalExtensions;
using PetFamily.Contracts;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Providers
{
    public interface IFileProvider
    {
        Task <Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
            IReadOnlyList<FileDTO> filesDTO, CancellationToken cancellationToken = default);

        Task<UnitResult<Error>> DeleteFiles(IEnumerable<FilePath> objectsNames, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetPresignedFile(string FileName, CancellationToken cancellationToken = default);
    }
}
