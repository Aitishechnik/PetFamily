using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Commands.ShiftPetPosition
{
    public record ShiftPetPositionCommand(
        Guid VolonteerId,
        Guid PetId,
        int NewPosition) : ICommand;
}
