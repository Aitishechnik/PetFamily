using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Queries.GetPetById
{
    public record GetPetByIdQuery(Guid PetId) : IQuery;
}
