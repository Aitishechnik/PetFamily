using PetFamily.Application.Volonteers.Commands.ShiftPetPosition;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record ShiftPetPositionRequest(int NewPosition)
    {
        public ShiftPetPositionCommand ToCommand(
            Guid volonteerId,
            Guid petId)
        {
            return new ShiftPetPositionCommand(
                volonteerId,
                petId,
                NewPosition);
        }
    }
}
