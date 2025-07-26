using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Species.Commands.RemoveSpecies
{
    public record RemoveSpeciesByIdCommand(Guid SpeciesId) : ICommand;
}
