using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos;
using PetFamily.Core.FileManagment.Files;
using PetFamily.SharedKernal;

namespace PetFamily.Core.FileManagement.Providers
{
    public interface IFileProvider
    {
        Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
            IReadOnlyList<FileDto> filesDTO, CancellationToken cancellationToken = default);

        Task<UnitResult<Error>> DeleteFiles(IEnumerable<FileInfoPath> objectsNames, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetPresignedFile(string FileName, CancellationToken cancellationToken = default);
    }
}
