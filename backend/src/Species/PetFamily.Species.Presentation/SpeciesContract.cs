using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation
{
    public class SpeciesContract : ISpeciesContract
    {
        private readonly ISpeciesReadDbContext _speciesReadDbContext;

        public SpeciesContract(ISpeciesReadDbContext speciesReadDbContext)
        {
            _speciesReadDbContext = speciesReadDbContext;
        }

        public async Task<bool> IsSpeciesAndBreedExist(
            IsSpeciesAndBreedExistRequest request, 
            CancellationToken cancellationToken)
        {
            return await _speciesReadDbContext.Breeds.AnyAsync(
                b => b.SpeciesId == request.SpeciesId &&
                b.Id == request.BreedId);
        }
    }
}
