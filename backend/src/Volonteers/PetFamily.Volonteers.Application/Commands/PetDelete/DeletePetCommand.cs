using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Commands.PetDelete
{
    public record DeletePetCommand(
        Guid VolonteerId,
        Guid PetId) : ICommand;
}
