using eCommerceAPI.Core.Contracts;
using eCommerceAPI.Infrastructures.Persistence.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace eCommerceAPI.Infrastructures.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task BeginTransactionAsync()
        {
            if (!HasActiveTransaction)
                _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (!HasActiveTransaction) 
                throw new InvalidOperationException("No active transaction");
            
            try
            {
                await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
            }catch
            {
                await _currentTransaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (HasActiveTransaction)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        {
            if (HasActiveTransaction)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }

            await _context.DisposeAsync();
        }
    }
}