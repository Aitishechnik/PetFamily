using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volonteers.GetById;

public class GetVolonteerByIdHandler : IQueryHandler<VolonteerDto, GetVolonteerByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolonteerByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<VolonteerDto> Handle(
        GetVolonteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var volonteersQuery = _readDbContext.Volonteers.AsQueryable();

        if (volonteersQuery.Count() < 1)
            return new VolonteerDto();

        var result = await volonteersQuery.Where(v => v.Id == query.VolonteerId).ToListAsync();

        if (result.Count < 1)
            return new VolonteerDto();

        return result[0];
    }
}
