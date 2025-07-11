using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Delete
{
    public record DeleteFilesRequest(IEnumerable<FilePath> ObjectsNames);
}
