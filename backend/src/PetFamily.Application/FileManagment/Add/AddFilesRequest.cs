using PetFamily.Contracts;

namespace PetFamily.Application.FileManagement.Add
{
    public record AddFilesRequest(IReadOnlyList<FileDTO> FilesDTO);
}
