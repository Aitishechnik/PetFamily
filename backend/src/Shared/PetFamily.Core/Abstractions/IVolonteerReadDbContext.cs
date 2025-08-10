using PetFamily.Core.Dtos;

namespace PetFamily.Core.Abstractions;

public interface IVolonteerReadDbContext
{
    IQueryable<VolonteerDto> Volonteers { get; }
    IQueryable<PetDto> Pets { get; }
}