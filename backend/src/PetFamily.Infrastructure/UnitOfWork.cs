using System.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
        {
            var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);
            return transaction.GetDbTransaction();
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public void EntryChangeStateOnAdded<TEntity>(TEntity entity) where TEntity : class
        {
            _appDbContext.Entry(entity).State = EntityState.Added;
        }

        public void EntryChangeStateOnModified<TEntity>(TEntity entity) where TEntity : class
        {
            _appDbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
