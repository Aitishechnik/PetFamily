using System.Data;
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
            using var transaction = await _appDbContext.Database
                .BeginTransactionAsync(cancellationToken);
            return transaction.GetDbTransaction();
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
