namespace Infrastructure.DataAccess;

using Application.Services;

public sealed class UnitOfWorkFake : IUnitOfWork
{
    public async Task<int> Save()
    {
        return await Task.FromResult(1)
            .ConfigureAwait(false);
    }
}