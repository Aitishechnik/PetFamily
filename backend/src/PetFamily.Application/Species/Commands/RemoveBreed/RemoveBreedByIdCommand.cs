using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Species.Commands.RemoveBreed
{
    public record RemoveBreedByIdCommand(Guid BreedId) : ICommand;
}
