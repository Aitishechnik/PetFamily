using PetFamily.Core.FileManagment.Files;

namespace PetFamily.Core.FileManagement.Delete
{
    public record DeleteFilesCommand(IEnumerable<FileInfoPath> filesInfo);
}
