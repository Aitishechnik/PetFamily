using PetFamily.Volonteers.Application.Queries.GetVolonteers;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record GetVolonteerWithPaginationRequest(int Page, int PageSize)
    {
        public GetVolonteersWithPaginationQuery ToQuery() =>
            new(Page, PageSize);

        public GetVolonteersWithPaginationQueryDapper ToQueryDapper() =>
            new(Page, PageSize);
    }
}
