using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace PetFamily.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IDbTransaction> BeginTransaction(
            CancellationToken cancellationToken = default)
        {
            var transaction = await _appDbContext.Database
                .BeginTransactionAsync(cancellationToken);
            return transaction.GetDbTransaction();
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync<TEntity>(
            TEntity entity, 
            CancellationToken cancellationToken = default) where TEntity : class
        {
            await _appDbContext.AddAsync(entity);
        }

        public IEnumerable<EntityEntry> ChangeTrackerEntry()
        {
            return _appDbContext.ChangeTracker.Entries();
        }

        public void Delete<TEntity>(
            TEntity entity, 
            CancellationToken cancellationToken = default) where TEntity : class
        {
            _appDbContext.Remove(entity);
        }
    }
}
