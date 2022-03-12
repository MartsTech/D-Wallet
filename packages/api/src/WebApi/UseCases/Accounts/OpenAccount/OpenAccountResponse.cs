namespace WebApi.UseCases.Accounts.OpenAccount;

using Application.UseCases.Accounts;

public sealed class OpenAccountResponse
{
    public OpenAccountResponse(AccountDto account)
    {
        Account = account;
    }

    public AccountDto Account { get; }
}