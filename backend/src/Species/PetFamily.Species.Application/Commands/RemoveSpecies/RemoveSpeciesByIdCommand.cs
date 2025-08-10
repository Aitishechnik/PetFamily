using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.RemoveSpecies
{
    public record RemoveSpeciesByIdCommand(Guid SpeciesId) : ICommand;
}
