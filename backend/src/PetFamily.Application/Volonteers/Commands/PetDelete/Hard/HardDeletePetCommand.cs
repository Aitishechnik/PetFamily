namespace PetFamily.Application.Volonteers.Commands.PetDelete.Hard
{
    public record HardDeletePetCommand : DeletePetCommand
    {
        public HardDeletePetCommand(Guid volonteerId, Guid petId) : base(volonteerId, petId)
        {
        }
    }
}
