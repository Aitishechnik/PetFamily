using PetFamily.Application.Abstraction;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.Application.Volonteers.Commands.RemovePetPhotos
{
    public record RemovePetPhotosCommand(
        Guid VolonteerId, 
        Guid PetId, 
        IEnumerable<FileInfo> FileInfo) : ICommand;
}
