using System.Data;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    Task SaveChanges(CancellationToken cancellationToken = default);
    void EntryChangeStateOnAdded<TEntity>(TEntity entity) where TEntity : class;
    void EntryChangeStateOnModified<TEntity>(TEntity entity) where TEntity : class;
}
