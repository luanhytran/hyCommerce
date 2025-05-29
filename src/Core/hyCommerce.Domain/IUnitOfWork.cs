namespace hyCommerce.Domain;

public interface IUnitOfWork : IAsyncDisposable
{
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task<int> SaveAsync();
    public Task RollbackAsync();
}