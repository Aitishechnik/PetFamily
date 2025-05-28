using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers
{
    public interface IVolonteersRepository
    {
        Task<Guid> Add(Volonteer volonteer, CancellationToken cancellationToken = default);

        Task<Result<Volonteer, Error>> GetById(Guid id);

        Task<Result<Volonteer, Error>> GetByEmail(string email);
    }
}
