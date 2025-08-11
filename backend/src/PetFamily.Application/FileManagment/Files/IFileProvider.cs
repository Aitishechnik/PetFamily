using CSharpFunctionalExtensions;
using PetFamily.Contracts;
using PetFamily.Domain.Shared;
using FileInfoPath = PetFamily.Application.FileManagment.Files.FileInfoPath;

namespace PetFamily.Application.FileManagement.Providers
{
    public interface IFileProvider
    {
        Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
            IReadOnlyList<FileDTO> filesDTO, CancellationToken cancellationToken = default);

        Task<UnitResult<Error>> DeleteFiles(IEnumerable<FileInfoPath> objectsNames, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetPresignedFile(string FileName, CancellationToken cancellationToken = default);
    }
}
