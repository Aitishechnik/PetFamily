using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Commands.Delete
{
    public record DeleteVolonteerCommand(Guid VolonteerId) : ICommand;
}
