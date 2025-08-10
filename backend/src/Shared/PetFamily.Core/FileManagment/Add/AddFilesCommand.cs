using PetFamily.Core.Dtos;

namespace PetFamily.Core.FileManagement.Add
{
    public record AddFilesCommand(IReadOnlyList<FileDto> FilesDTO);
}
