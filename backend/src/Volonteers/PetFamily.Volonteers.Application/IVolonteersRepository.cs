using CSharpFunctionalExtensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.Entities;

namespace PetFamily.Volonteers.Application
{
    public interface IVolonteersRepository
    {
        Task<Guid> Add(Volonteer volonteer, CancellationToken cancellationToken = default);

        Task<Result<Volonteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

        Task<Result<Volonteer, Error>> GetByEmail(string email, CancellationToken cancellationToken = default);

        Task<Result<Guid, Error>> Delete(Volonteer volonteer, CancellationToken cancellationToken = default);
    }
}
