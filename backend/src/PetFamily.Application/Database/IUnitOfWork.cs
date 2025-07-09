using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    Task SaveChanges(CancellationToken cancellationToken = default);
    Task AddAsync<TEntity>(
        TEntity entity, 
        CancellationToken cancellationToken = default) where TEntity : class;
    void Delete<TEntity>(
            TEntity entity,
            CancellationToken cancellationToken = default) where TEntity : class;
    IEnumerable<EntityEntry> ChangeTrackerEntry();
}
