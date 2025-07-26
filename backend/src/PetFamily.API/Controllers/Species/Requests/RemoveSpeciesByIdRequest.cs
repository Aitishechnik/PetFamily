using PetFamily.Application.Species.Commands.RemoveSpecies;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record RemoveSpeciesByIdRequest(Guid SpeciesId)
    {
        public RemoveSpeciesByIdCommand ToCommand() =>
            new(SpeciesId);
    }
}
