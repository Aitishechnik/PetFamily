namespace PetFamily.Application.Volonteers.Commands.Delete.Hard
{
    public record HardDeleteVolonteerCommand : DeleteVolonteerCommand
    {
        public HardDeleteVolonteerCommand(Guid VolonteerId) : base(VolonteerId)
        {
        }
    }
}
