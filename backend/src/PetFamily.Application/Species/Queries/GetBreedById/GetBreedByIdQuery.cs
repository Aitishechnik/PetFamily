using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Species.Queries.GetBreedById
{
    public record GetBreedByIdQuery(Guid SpeciedId, Guid BreedId) : IQuery;
}
