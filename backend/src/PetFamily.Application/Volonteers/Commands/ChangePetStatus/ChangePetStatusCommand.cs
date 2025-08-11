using PetFamily.Application.Abstraction;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers.Commands.ChangePetStatus
{
    public record ChangePetStatusCommand(
        Guid VolonteerId,
        Guid PetId,
        HelpStatus NewPetStatus) : ICommand;
}
