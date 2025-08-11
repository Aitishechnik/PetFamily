using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Application.GetById;

public class GetVolonteerByIdHandler : IQueryHandler<VolonteerDto, GetVolonteerByIdQuery>
{
    private readonly IVolonteerReadDbContext _readDbContext;

    public GetVolonteerByIdHandler(IVolonteerReadDbContext readDbContext)
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
