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
        public async Task<UnitResult<Error>> IsSpeciesAndBreedExists(
            Guid speciesId, 
            Guid breedId, 
            CancellationToken cancellationToken = default)
        {
            var speciesExists = await _dbContext.Species
                .AnyAsync(s => s.Id == speciesId, cancellationToken);

            if(speciesExists == false)
                return Errors.General.NotFound(speciesId);

            var breedExists = await _dbContext.Species
                .AnyAsync(s => s.Id == speciesId && 
                s.Breeds.Any(b => b.Id == breedId), 
                cancellationToken);

            if(breedExists == false)
                return Errors.General.ValueIsInvalid("breed");

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> RemoveSpecies(Guid speciesId)
        {
            var species = await _dbContext.Species.FindAsync(speciesId);

            if (species is null)
                return Errors.General.NotFound(speciesId);

            _dbContext.Remove(species);

            return Result.Success<Error>();
        }
    }
}
