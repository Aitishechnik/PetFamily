using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Enums;

namespace PetFamily.Volonteers.Application.Commands.ChangePetStatus
{
    public record ChangePetStatusCommand(
        Guid VolonteerId,
        Guid PetId,
        HelpStatus NewPetStatus) : ICommand;
}
