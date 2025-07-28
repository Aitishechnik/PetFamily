namespace PetFamily.Application.Volonteers.Commands.Delete.Soft
{
    public record SoftDeleteVolonteerCommand : DeleteVolonteerCommand
    {
        public SoftDeleteVolonteerCommand(Guid VolonteerId) : base(VolonteerId) { }
    }
}
