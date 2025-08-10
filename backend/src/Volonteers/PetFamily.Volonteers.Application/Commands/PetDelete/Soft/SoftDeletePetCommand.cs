namespace PetFamily.Volonteers.Application.Commands.PetDelete.Soft
{
    public record SoftDeletePetCommand : DeletePetCommand
    {
        public SoftDeletePetCommand(Guid VolonteerId, Guid PetId) : base(VolonteerId, PetId)
        {
        }
    }
}
