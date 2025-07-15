using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.Application.FileManagement.Delete
{
    public record DeleteFilesRequest(IEnumerable<FileInfo> filesInfo);
}
