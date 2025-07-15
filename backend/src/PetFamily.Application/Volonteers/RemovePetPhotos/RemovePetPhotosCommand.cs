using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.Application.Volonteers.RemovePetPhotos
{
    public record RemovePetPhotosCommand(Guid VolonteerId, Guid PetId, IEnumerable<FileInfo> FileInfo);
}
