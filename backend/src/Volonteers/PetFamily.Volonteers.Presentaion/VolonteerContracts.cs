using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Volonteers.Contracts;
using PetFamily.Volonteers.Contracts.Requests;

namespace PetFamily.Volonteers.Presentation;

public class VolunteerContract : IVolonteerContract
{
    private readonly IVolonteerReadDbContext _readDbContext;

    public VolunteerContract(IVolonteerReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<bool> HasAnimalsWithBreed(HasPetsWithBreedRequest request, CancellationToken cancellationToken) => 
        await _readDbContext.Pets.AnyAsync(p => p.BreedId == request.BreedId);
    

    public async Task<bool> HasAnimalsWithSpecies(HasPetsWithSpeciesRequest request, CancellationToken cancellationToken) =>
        await _readDbContext.Pets.AnyAsync(p => p.SpeciesId == request.SpeciesId);
}