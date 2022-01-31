namespace Application.UseCases.GetAccount;

using Domain.Accounts;
using Domain.ValueObjects;

public sealed class GetAccountUseCase : IGetAccountUseCase
{
    private readonly IAccountRepository _accountRepository;
    private IOutputPort _outputPort;

    public GetAccountUseCase(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
        _outputPort = new GetAccountPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }

    public Task Execute(Guid accountId)
    {
        return GetAccountInternal(new AccountId(accountId));
    }

    private async Task GetAccountInternal(AccountId accountId)
    {
        IAccount account = await _accountRepository
            .GetAccount(accountId)
            .ConfigureAwait(false);

        if (account is Account getAccount)
        {
            _outputPort.Ok(getAccount);
            return;
        }

        _outputPort.NotFound();
    }
}
