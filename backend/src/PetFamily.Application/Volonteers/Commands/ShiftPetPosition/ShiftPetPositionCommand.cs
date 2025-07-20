
using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Commands.ShiftPetPosition
{
    public record ShiftPetPositionCommand(
        Guid VoloteerId, 
        Guid PetId, 
        int NewPosition) : ICommand;
}
