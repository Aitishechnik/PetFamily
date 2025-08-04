using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Species.Commands.AddBreed
{
    public record AddBreedCommand(Guid speciesId, string Name) : ICommand;
}
