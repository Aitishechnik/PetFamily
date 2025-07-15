using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.Application.Volonteers.RemovePetPhotos
{
    public record RemovePetPhotosRequest(Guid VolonteerId, Guid PetId, IEnumerable<FileInfo> FileInfo);
}
