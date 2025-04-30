using hyCommerce.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace hyCommerce.Infrastructure.Persistence;

public interface IUnitOfWork : IAsyncDisposable
{
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task<int> SaveAsync();
    public Task RollbackAsync();
}

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    private bool HasActiveTransaction => _currentTransaction != null;

    public async Task BeginTransactionAsync()
    {
        if (!HasActiveTransaction)
            _currentTransaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (!HasActiveTransaction)
            throw new InvalidOperationException("No active transaction");

        try
        {
            await context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
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

    public async Task<int> SaveAsync() => await context.SaveChangesAsync();

    public async ValueTask DisposeAsync()
    {
        if (HasActiveTransaction)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        await context.DisposeAsync();
    }
}