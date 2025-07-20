using PetFamily.Application.Volonteers.GetById;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record GetVolonteerByIdRequest(Guid VolonteerId)
    {
        public GetVolonteerByIdQuery ToQuery() =>
            new(VolonteerId);
    }
}
