namespace UnitTests;

using Infrastructure.CurrencyExchange;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Factories;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.ExternalAuthentication;

public sealed class StandardFixture
{
    public StandardFixture()
    {
        AccountFactory = new AccountFactory();
        Context = new DataContextFake();
        AccountRepositoryFake = new AccountRepositoryFake(Context);
        UnitOfWork = new UnitOfWorkFake();
        TestUserService = new TestUserService();
        CurrencyExchangeFake = new CurrencyExchangeFake();
    }

    public AccountFactory AccountFactory { get; }

    public DataContextFake Context { get; }

    public AccountRepositoryFake AccountRepositoryFake { get; }

    public UnitOfWorkFake UnitOfWork { get; }

    public TestUserService TestUserService { get; }

    public CurrencyExchangeFake CurrencyExchangeFake { get; }

}
