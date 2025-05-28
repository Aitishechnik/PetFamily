using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers
{
    public interface IVolonteersRepository
    {
        Task<Guid> Add(Volonteer volonteer, CancellationToken cancellationToken = default);
    }
}
