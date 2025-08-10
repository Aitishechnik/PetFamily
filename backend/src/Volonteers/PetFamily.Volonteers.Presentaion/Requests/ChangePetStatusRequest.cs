using PetFamily.Core.Dtos.Enums;
using PetFamily.Volonteers.Application.Commands.ChangePetStatus;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record ChangePetStatusRequest(HelpStatus NewPetStatus)
    {
        public ChangePetStatusCommand ToCommand(Guid volonteerId, Guid petId)
            => new(volonteerId, petId, NewPetStatus);

    }
}
