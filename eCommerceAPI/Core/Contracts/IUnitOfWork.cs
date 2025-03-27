using Microsoft.EntityFrameworkCore.Storage;

namespace eCommerceAPI.Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task<int> SaveAsync();
        public Task RollbackAsync();
    }
}
