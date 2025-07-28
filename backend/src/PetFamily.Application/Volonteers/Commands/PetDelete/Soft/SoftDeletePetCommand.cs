namespace PetFamily.Application.Volonteers.Commands.PetDelete.Soft
{
    public record SoftDeletePetCommand : DeletePetCommand
    {
        public SoftDeletePetCommand(Guid VolonteerId, Guid PetId) : base(VolonteerId, PetId)
        {
        }
    }
}
