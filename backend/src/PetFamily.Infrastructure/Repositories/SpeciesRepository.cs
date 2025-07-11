using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly AppDbContext _appDbContext;

        public SpeciesRepository(AppDbContext appDbContext)
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
