namespace Infrastructure.DataAccess;

using Application.Services;
using Persistence;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DataContext _context;
    private bool _disposed;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public async Task<int> Save()
    {
        int affectedRows = await _context
           .SaveChangesAsync()
           .ConfigureAwait(false);

        return affectedRows;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}