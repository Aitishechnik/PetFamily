using PetFamily.Application.Volonteers;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolonteersRepository : IVolonteersRepository
    {
        private readonly AppDbContext _appDbContext;
        public VolonteersRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Add(Volonteer volonteer, CancellationToken cancellationToken = default)
        {
            await _appDbContext.Volonteers.AddAsync(volonteer, cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return volonteer.Id;
        }
    }
}
