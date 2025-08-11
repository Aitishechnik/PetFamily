namespace PetFamily.Volonteers.Application.Commands.Delete.Hard
{
    public record HardDeleteVolonteerCommand : DeleteVolonteerCommand
    {
        public HardDeleteVolonteerCommand(Guid VolonteerId) : base(VolonteerId)
        {
        }
    }
}
