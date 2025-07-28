using PetFamily.Application.Volonteers.Commands.ChangePetStatus;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record ChangePetStatusRequest(HelpStatus NewPetStatus)
    {
        public ChangePetStatusCommand ToCommand(Guid volonteerId, Guid petId)
            => new (volonteerId, petId, NewPetStatus);
        
    }
}
