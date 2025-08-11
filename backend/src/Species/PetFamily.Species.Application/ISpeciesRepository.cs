using CSharpFunctionalExtensions;
using PetFamily.SharedKernal;
using PetFamily.Species.Domain.Entities;

namespace PetFamily.Species.Application
{
    public interface ISpeciesRepository
    {
        Task<UnitResult<Error>> RemoveSpecies(Guid speciesId,
            CancellationToken cancellationToken = default);
        Task<Result<Domain.Entities.Species, Error>> GetSpeciesById(Guid speciesId,
            CancellationToken cancellationToken = default);
        Task<Result<Breed, Error>> GetBreedById(Guid breedId,
            CancellationToken cancellationToken = default);
        Task<UnitResult<Error>> RemoveBreed(
            Guid breedId,
            CancellationToken cancellationToken = default);
        Task AddSpecies(
            Domain.Entities.Species species,
            CancellationToken cancellationToken = default);

    }
}
