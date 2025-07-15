using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagment.Files
{
    public record FileInfo(string Bucket, FilePath FilePath);
}
