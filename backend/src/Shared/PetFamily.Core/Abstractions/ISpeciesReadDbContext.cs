using PetFamily.Core.Dtos;

namespace PetFamily.Core.Abstractions
{
    public interface ISpeciesReadDbContext
    {
        IQueryable<SpeciesDto> Species { get; }
        IQueryable<BreedDto> Breeds { get; }
    }
}
