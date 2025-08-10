using PetFamily.Species.Application.Commands.RemoveSpecies;

namespace PetFamily.Species.Requests
{
    public record RemoveSpeciesByIdRequest(Guid SpeciesId)
    {
        public RemoveSpeciesByIdCommand ToCommand() =>
            new(SpeciesId);
    }
}
