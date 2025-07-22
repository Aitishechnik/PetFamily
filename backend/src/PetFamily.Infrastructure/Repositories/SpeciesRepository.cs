using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly WriteDbContext _appDbContext;

        public SpeciesRepository(WriteDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<UnitResult<Error>> IsSpeciesAndBreedExists(
            Guid speciesId, 
            Guid breedId, 
            CancellationToken cancellationToken = default)
        {
            var speciesExists = await _appDbContext.Species
                .AnyAsync(s => s.Id == speciesId, cancellationToken);

            if(speciesExists == false)
                return Errors.General.NotFound(speciesId);

            var breedExists = await _appDbContext.Species
                .AnyAsync(s => s.Id == speciesId && 
                s.Breeds.Any(b => b.Id == breedId), 
                cancellationToken);

            if(breedExists == false)
                return Errors.General.ValueIsInvalid("breed");

            return UnitResult.Success<Error>();
        }
    }
}
