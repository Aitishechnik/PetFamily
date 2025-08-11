using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly WriteDbContext _dbContext;

        public SpeciesRepository(WriteDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<Result<Species, Error>> GetSpeciesById(
            Guid speciesId,
            CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Species.FindAsync(
                speciesId,
                cancellationToken);
            if (result is null)
                return Errors.General.NotFound(speciesId);
            return result;
        }

        public async Task<UnitResult<Error>> RemoveSpecies(
            Guid speciesId,
            CancellationToken cancellationToken = default)
        {
            var species = await _dbContext.Species.FindAsync(
                speciesId,
                cancellationToken);

            if (species is null)
                return Errors.General.NotFound(speciesId);

            _dbContext.Remove(species);

            return Result.Success<Error>();
        }

        public async Task<Result<Breed, Error>> GetBreedById(
            Guid breedId,
            CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Species
                .SelectMany(s => s.Breeds)
                .FirstAsync(b => b.Id == breedId, cancellationToken);
            if (result is null)
                return Errors.General.NotFound(breedId);

            return result;
        }

        public async Task<UnitResult<Error>> RemoveBreed(
            Guid breedId,
            CancellationToken cancellationToken = default)
        {
            var breed = await _dbContext.Species
                .SelectMany(s => s.Breeds)
                .FirstOrDefaultAsync(b => b.Id == breedId, cancellationToken);
            if (breed is null)
                return Errors.General.NotFound(breedId);

            _dbContext.Remove(breed);

            return Result.Success<Error>();
        }

        public async Task AddSpecies(
            Species species,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Species.AddAsync(species);
        }
    }
}
