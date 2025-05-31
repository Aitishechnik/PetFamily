using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

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

        public async Task<Result<Volonteer, Error>> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            var result = await _appDbContext.Volonteers
                .FirstOrDefaultAsync(v => v.PersonalData.Email == email);

            if (result is null)
                return Errors.General.NotFound();

            return result;
        }

        public async Task<Result<Volonteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _appDbContext.Volonteers
                .FirstOrDefaultAsync(v => v.Id == id);

            if (result is null)
                return Errors.General.NotFound(id);

            return result;
        }
    }
}
