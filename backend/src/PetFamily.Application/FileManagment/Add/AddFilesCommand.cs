using PetFamily.Contracts;

namespace PetFamily.Application.FileManagement.Add
{
    public record AddFilesCommand(IReadOnlyList<FileDTO> FilesDTO);
}
