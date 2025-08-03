using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Species.Commands.AddSpecies
{
    public record AddSpeciesCommand(string Name) : ICommand;
}
