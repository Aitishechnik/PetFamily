using PetFamily.Volonteers.Application.Commands.ShiftPetPosition;

namespace PetFamily.Volonteers.Presentation.Requests
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
