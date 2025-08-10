using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Commands.PetDelete
{
    public record DeletePetCommand(
        Guid VolonteerId,
        Guid PetId) : ICommand;
}
