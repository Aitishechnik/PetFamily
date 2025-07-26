using PetFamily.Application.Dtos;

namespace PetFamily.Application.Database
{
    public interface IReadDbContext
    {
        IQueryable<VolonteerDto> Volonteers { get; }
        IQueryable<SpeciesDto> Species { get; }
        IQueryable<BreedDto> Breeds { get; }
        IQueryable<PetDto> Pets { get; }
    }
}