using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Queries.GetVolonteers
{
    public record GetVolonteersWithPaginationQuery(int Page, int PageSize) : IQuery;
}
