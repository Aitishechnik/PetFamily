using PetFamily.Application.Species.Queries.GetBreedById;

namespace PetFamily.Species.Requests
{
    public record GetBreedByIdRequest(Guid SpeciesId, Guid BreedId)
    {
        public GetBreedByIdQuery ToQuery() =>
            new(SpeciesId, BreedId);
    }
}
