namespace PetFamily.Application.Volonteers.ShiftPetPosition
{
    public record ShiftPetPositionRequest(
        Guid VoloteerId, 
        Guid PetId, 
        int NewPosition);
}
