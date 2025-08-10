using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Commands.SetPetMainPhoto
{
    public record SetPetMainPhotoCommand(
        Guid VolonteerId,
        Guid PetId,
        string MainPhotoPath) : ICommand;
}
