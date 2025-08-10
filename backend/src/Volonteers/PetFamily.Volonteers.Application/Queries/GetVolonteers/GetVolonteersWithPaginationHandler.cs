using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volonteers.Application.Queries.GetVolonteers
{
    public class GetVolonteersWithPaginationHandler : IQueryHandler<PagedList<VolonteerDto>, GetVolonteersWithPaginationQuery>
    {
        private readonly IVolonteerReadDbContext _readDbContext;

        public GetVolonteersWithPaginationHandler(IVolonteerReadDbContext readDbContext)
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
