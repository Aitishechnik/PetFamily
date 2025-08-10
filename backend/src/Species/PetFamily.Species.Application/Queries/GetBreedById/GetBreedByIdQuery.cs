using PetFamily.Core.Abstractions;

namespace PetFamily.Application.Species.Queries.GetBreedById
{
    public record GetBreedByIdQuery(Guid SpeciedId, Guid BreedId) : IQuery;
}
