using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolonteersRepository : IVolonteersRepository
    {
        private readonly WriteDbContext _appDbContext;
        public VolonteersRepository(WriteDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        public async Task<IEnumerable<Pet>> GetAllPets()
        {
            return await _appDbContext.Volonteers
                .SelectMany(v => v.Pets)
                .ToListAsync();
        }

        public async Task<Guid> Add(Volonteer volonteer, CancellationToken cancellationToken = default)
        {
            await _appDbContext.Volonteers.AddAsync(volonteer, cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return volonteer.Id;
        }

        public async Task<Result<Guid, Error>> Delete(Volonteer volonteer, CancellationToken cancellationToken = default)
        {
            var result = await _appDbContext.Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteer.Id);

            if (result is null)
                return Errors.General.NotFound();

            _appDbContext.Volonteers.Remove(volonteer);

            await _appDbContext.SaveChangesAsync();

            return result.Id;
        }

        public async Task<Result<Volonteer, Error>> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            var result = await _appDbContext.Volonteers
                .Include(p => p.Pets)
                .FirstOrDefaultAsync(v => v.PersonalData.Email == email);

            if (result is null)
                return Errors.General.NotFound();

            return result;
        }

        public async Task<Result<Volonteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _appDbContext.Volonteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

            if (result is null)
                return Errors.General.NotFound(id);

            return result;
        }
    }
}
