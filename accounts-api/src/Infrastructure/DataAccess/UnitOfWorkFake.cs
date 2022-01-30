using Application.Services;

namespace Infrastructure.DataAccess;

public sealed class UnitOfWorkFake : IUnitOfWork
{
    public async Task<int> Save()
    {
        return await Task.FromResult(0)
            .ConfigureAwait(false);
    }
}
