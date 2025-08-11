using FileInfoPath = PetFamily.Application.FileManagment.Files.FileInfoPath;

namespace PetFamily.Application.FileManagement.Delete
{
    public record DeleteFilesCommand(IEnumerable<FileInfoPath> filesInfo);
}
