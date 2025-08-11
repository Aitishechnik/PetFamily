using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Queries.GetVolonteers
{
    public record GetVolonteersWithPaginationQuery(int Page, int PageSize) : IQuery;
}
