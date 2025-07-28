using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Commands.SetPetMainPhoto
{
    public record SetPetMainPhotoCommand(
        Guid VolonteerId,
        Guid PetId,
        string MainPhotoPath) : ICommand;
}
