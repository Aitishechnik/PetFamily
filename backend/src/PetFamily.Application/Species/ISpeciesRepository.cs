using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species
{
    public interface ISpeciesRepository
    {
        Task<UnitResult<Error>> IsSpeciesAndBreedExists(
            Guid speciesId, 
            Guid breedId, 
            CancellationToken cancellationToken = default);
    }
}
