using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Queries.GetVolonteers
{
    public record GetVolonteersWithPaginationQueryDapper(int Page, int PageSize) : IQuery;
}
