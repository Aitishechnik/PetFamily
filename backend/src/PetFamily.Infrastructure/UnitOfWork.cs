using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Infrastructure.DbContexts;
using System.Data;

namespace PetFamily.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WriteDbContext _appDbContext;

        public UnitOfWork(WriteDbContext appDbContext)
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
