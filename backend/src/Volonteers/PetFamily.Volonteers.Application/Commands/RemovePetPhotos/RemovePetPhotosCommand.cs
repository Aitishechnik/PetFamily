using PetFamily.Core.Abstractions;
using PetFamily.Core.FileManagment.Files;

namespace PetFamily.Volonteers.Application.Commands.RemovePetPhotos
{
    public record RemovePetPhotosCommand(
        Guid VolonteerId,
        Guid PetId,
        IEnumerable<FileInfoPath> FileInfoPath) : ICommand;
}
