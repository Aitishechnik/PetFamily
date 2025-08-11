using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.GetById
{
    public record GetVolonteerByIdQuery(Guid VolonteerId) : IQuery;
}
