using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Commands.Delete
{
    public abstract record DeleteVolonteerCommand(Guid VolonteerId) : ICommand;
}
