using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species
{
    public interface ISpeciesRepository
    {
        Task<UnitResult<Error>> RemoveSpecies(Guid speciesId, 
            CancellationToken cancellationToken = default);
        Task<Result<Domain.Models.Species.Species, Error>> GetSpeciesById(Guid speciesId, 
            CancellationToken cancellationToken = default);
        Task<Result<Breed, Error>> GetBreedById(Guid breedId, 
            CancellationToken cancellationToken = default);
        Task<UnitResult<Error>> RemoveBreed(
            Guid breedId,
            CancellationToken cancellationToken = default);


    }
}
