using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volonteers.Queries.GetVolonteers
{
    public class GetVolonteersWithPaginationHandler : IQueryHandler<PagedList<VolonteerDto>, GetVolonteersWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetVolonteersWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolonteerDto>> Handle(
            GetVolonteersWithPaginationQuery query,
            CancellationToken cancellationToken)
        {
            var volonteersQuery = _readDbContext.Volonteers.AsQueryable();

            return await volonteersQuery
                .ToPagedList(
                query.Page, 
                query.PageSize, 
                cancellationToken);
        }
    }
}
