using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Commands.PetDelete
{
    public abstract record DeletePetCommand(
        Guid VolonteerId, 
        Guid PetId) : ICommand;
}
