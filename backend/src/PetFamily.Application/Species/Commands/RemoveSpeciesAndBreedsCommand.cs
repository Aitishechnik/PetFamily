using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Species.Commands
{
    public record RemoveSpeciesAndBreedsCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}
