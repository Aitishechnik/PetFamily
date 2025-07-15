namespace PetFamily.Application.Volonteers.ShiftPetPosition
{
    public record ShiftPetPositionCommand(
        Guid VoloteerId, 
        Guid PetId, 
        int NewPosition);
}
