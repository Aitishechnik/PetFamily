using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Commands.Delete
{
    public record DeleteVolonteerCommand(Guid VolonteerId) : ICommand;
}
