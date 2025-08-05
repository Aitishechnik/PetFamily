using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Queries.GetPetById
{
    public record GetPetByIdQuery(Guid PetId) : IQuery;
}
