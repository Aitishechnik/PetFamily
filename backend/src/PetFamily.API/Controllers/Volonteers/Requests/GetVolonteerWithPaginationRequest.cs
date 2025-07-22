using PetFamily.Application.Volonteers.Queries.GetVolonteers;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record GetVolonteerWithPaginationRequest(int Page, int PageSize)
    {
        public GetVolonteersWithPaginationQuery ToQuery() =>
            new(Page, PageSize);

        public GetVolonteersWithPaginationQueryDapper ToQueryDapper() =>
            new(Page, PageSize);
    }
}
