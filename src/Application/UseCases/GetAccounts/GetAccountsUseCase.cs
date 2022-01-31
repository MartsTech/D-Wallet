namespace Application.UseCases.GetAccounts;

using Application.Services;
using Domain.Accounts;

public sealed class GetAccountsUseCase : IGetAccountsUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserService _userService;
    private IOutputPort _outputPort;

    public GetAccountsUseCase(IAccountRepository accountRepository, IUserService userService)
    {
        _accountRepository = accountRepository;
        _userService = userService;
        _outputPort = new GetAccountPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }

    public Task Execute()
    {
        string externalUserId = _userService.GetCurrentUserId();

        return GetAccounts(externalUserId);
    }

    private async Task GetAccounts(string externalUserId)
    {
        IList<Account> accounts = await _accountRepository
           .GetAccounts(externalUserId)
           .ConfigureAwait(false);

        _outputPort.Ok(accounts);
    }
}
