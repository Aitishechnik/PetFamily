using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Queries.GetVolonteers
{
    public record GetVolonteersWithPaginationQueryDapper(int Page, int PageSize) : IQuery;
}
