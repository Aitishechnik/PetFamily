using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Infrastructure.DbContexts;
using System.Data;

namespace PetFamily.Species.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpeciesWriteDbContext _appDbContext;

        public UnitOfWork(SpeciesWriteDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IDbTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            var transaction = await _appDbContext.Database
                .BeginTransactionAsync(cancellationToken);
            return transaction.GetDbTransaction();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
