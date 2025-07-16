using PetFamily.Application.Volonteers.ShiftPetPosition;

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
