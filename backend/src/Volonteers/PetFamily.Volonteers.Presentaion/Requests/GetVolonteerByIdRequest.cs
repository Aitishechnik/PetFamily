using PetFamily.Volonteers.Application.GetById;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record GetVolonteerByIdRequest(Guid VolonteerId)
    {
        public GetVolonteerByIdQuery ToQuery() =>
            new(VolonteerId);
    }
}
