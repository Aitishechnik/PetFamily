using PetFamily.Application.Abstraction;
using FileInfoPath = PetFamily.Application.FileManagment.Files.FileInfoPath;

namespace PetFamily.Application.Volonteers.Commands.RemovePetPhotos
{
    public record RemovePetPhotosCommand(
        Guid VolonteerId,
        Guid PetId,
        IEnumerable<FileInfoPath> FileInfo) : ICommand;
}
