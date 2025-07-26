using PetFamily.Application.Species.Commands;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record RemoveSpeciesAndBreedsRequest(Guid SpeciesId, Guid BreedId)
    {
        public RemoveSpeciesAndBreedsCommand ToCommand() =>
            new(SpeciesId, BreedId);
    }
}
