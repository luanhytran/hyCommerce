using Microsoft.EntityFrameworkCore.Storage;

namespace hyCommerce.Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task<int> SaveAsync();
        public Task RollbackAsync();
    }
}
