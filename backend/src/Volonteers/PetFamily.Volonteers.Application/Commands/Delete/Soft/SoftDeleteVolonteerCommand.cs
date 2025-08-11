namespace PetFamily.Volonteers.Application.Commands.Delete.Soft
{
    public record SoftDeleteVolonteerCommand : DeleteVolonteerCommand
    {
        public SoftDeleteVolonteerCommand(Guid VolonteerId) : base(VolonteerId) { }
    }
}
