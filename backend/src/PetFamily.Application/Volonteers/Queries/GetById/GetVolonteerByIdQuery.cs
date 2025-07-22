using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.GetById
{
    public record GetVolonteerByIdQuery(Guid VolonteerId) : IQuery;
}
