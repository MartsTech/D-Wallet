namespace UnitTests;

using Infrastructure.CurrencyExchange;
using Infrastructure.DataAccess;
using Infrastructure.Security;
using Persistence;
using Persistence.Factories;
using Persistence.Repositories;

public sealed class StandardFixture
{
    public StandardFixture()
    {
        EntityFactory = new EntityFactory();
        Context = new DataContextFake();
        AccountRepository = new AccountRepositoryFake(Context);
        UnitOfWork = new UnitOfWorkFake();
        UserService = new TestUserService();
        CurrencyExchange = new CurrencyExchangeFake();
    }

    public EntityFactory EntityFactory { get; }

    public DataContextFake Context { get; }

    public AccountRepositoryFake AccountRepository { get; }

    public UnitOfWorkFake UnitOfWork { get; }

    public TestUserService UserService { get; }

    public CurrencyExchangeFake CurrencyExchange { get; }
}